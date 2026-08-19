using GitReview.Core.Exceptions;
using GitReview.Shared.Enums;
using GitReview.Shared.Extensions;
using GitReview.Shared.Providers;
using System.Net;

namespace GitReview.Core.Services;

public abstract class BaseLlmService : ILlmReviewService
{
    protected abstract AiProvider Provider { get; }

    protected readonly HttpClient HttpClient;
    protected string ProviderName => Provider.GetDescription();

    protected BaseLlmService(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    public async Task<string> GetReviewAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var apiKeyEnvVar = Provider.GetApiKeyEnvVar();
        var apiKey = Environment.GetEnvironmentVariable(apiKeyEnvVar);

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException($"API Key is missing. Please set the '{apiKeyEnvVar}' environment variable.");
        }

        var model = Provider.GetModel();

        Console.WriteLine($"📡 Model: {model} ({ProviderName})");

        try
        {
            using var request = CreateRequest(Provider.GetEndpoint(), model, apiKey, prompt);
            using var response = await HttpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponseAsync(response, cancellationToken);
            }

            var textResult = await ExtractTextResponseAsync(response, cancellationToken);
            if (string.IsNullOrWhiteSpace(textResult))
            {
                throw new InvalidOperationException($"Received empty text response from {ProviderName} API.");
            }

            return textResult;
        }
        catch (OperationCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            throw new LlmTimeoutException("Request timed out while waiting for LLM response.", ex);
        }
        catch (HttpRequestException ex)
        {
            throw new LlmApiException($"Network error occurred while calling {ProviderName} API: {ex.Message}", ex);
        }
    }

    protected abstract HttpRequestMessage CreateRequest(string endpoint, string model, string apiKey, string prompt);

    protected abstract Task<string?> ExtractTextResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken);

    private async Task HandleErrorResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var errorPayload = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            throw new LlmApiException(
                $"{ProviderName} rate limit hit ({(int)HttpStatusCode.TooManyRequests}). The shared free pool for this model is temporarily exhausted.\n" +
                "💡 Try switching to another model or provider.",
                null);
        }

        throw new LlmApiException($"{ProviderName} API error [{response.StatusCode}]: {errorPayload}", null);
    }
}
