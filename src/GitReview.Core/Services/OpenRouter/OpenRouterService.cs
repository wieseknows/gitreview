using GitReview.Core.Exceptions;
using GitReview.Core.Services.OpenRouter.Dto;
using GitReview.Shared.Enums;
using GitReview.Shared.Providers;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GitReview.Core.Services.OpenRouter;

public sealed class OpenRouterService : ILlmReviewService
{
    private const string Endpoint = "https://openrouter.ai/api/v1/chat/completions";

    private readonly HttpClient _httpClient;

    public OpenRouterService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetReviewAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var apiKey = Environment.GetEnvironmentVariable(AiProvider.OpenRouter.GetApiKeyEnvVar());
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                $"API Key is missing. Please set the '{AiProvider.OpenRouter.GetApiKeyEnvVar()}' environment variable.");
        }

        var model = AiProvider.OpenRouter.GetModel();
        var requestBody = new OpenRouterRequest(
            Model: model,
            Messages:
            [
                new OpenRouterMessage("system", "You are an expert Senior Code Reviewer."),
                new OpenRouterMessage("user", prompt)
            ]
        );

        Console.WriteLine($"📡 Model: {model} (OpenRouter)");

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, Endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            request.Content = JsonContent.Create(requestBody);

            using var response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorPayload = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException($"OpenRouter API error [{response.StatusCode}]: {errorPayload}");
            }

            var result = await response.Content.ReadFromJsonAsync<OpenRouterResponse>(cancellationToken: cancellationToken);
            var textResult = result?.Choices?
                .Select(c => c.Message?.Content)
                .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));

            if (string.IsNullOrWhiteSpace(textResult))
            {
                throw new InvalidOperationException("Received empty text response from OpenRouter API.");
            }

            return textResult;
        }
        catch (OperationCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            // Timeout triggered by HttpClient.Timeout
            throw new LlmTimeoutException("Request timed out while waiting for LLM response.", ex);
        }
        catch (HttpRequestException ex)
        {
            // Network failure / DNS / Connection drops
            throw new LlmApiException($"Network error occurred while calling OpenRouter API: {ex.Message}", ex);
        }
    }
}