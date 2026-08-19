using GitReview.Core.Exceptions;
using GitReview.Core.Services.Gemini.Dto;
using GitReview.Shared.Enums;
using GitReview.Shared.Providers;
using System.Net.Http.Json;

namespace GitReview.Core.Services.Gemini;

public sealed class GeminiService : ILlmReviewService
{
    private const string EndPoint = "https://generativelanguage.googleapis.com/v1beta";

    private readonly HttpClient _httpClient;

    public GeminiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetReviewAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var apiKey = Environment.GetEnvironmentVariable(AiProvider.Gemini.GetApiKeyEnvVar());
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                $"API Key is missing. Please set the '{AiProvider.Gemini.GetApiKeyEnvVar()}' environment variable.");
        }

        var model = AiProvider.Gemini.GetModel();
        var requestBody = new GeminiRequest(
        [
            new GeminiContent(
            [
                new GeminiPart(prompt)
            ])
        ]);

        Console.WriteLine($"📡 Model: {model}");

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{EndPoint}/models/{model}:generateContent");
            request.Headers.TryAddWithoutValidation("x-goog-api-key", apiKey);
            request.Content = JsonContent.Create(requestBody);

            using var response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorPayload = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException($"Gemini API error [{response.StatusCode}]: {errorPayload}");
            }

            var result = await response.Content.ReadFromJsonAsync<GeminiResponse>(cancellationToken: cancellationToken);
            var textResult = result?.Candidates?
                .SelectMany(c => c.Content?.Parts ?? [])
                .Select(p => p.Text)
                .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));

            if (string.IsNullOrWhiteSpace(textResult))
            {
                throw new InvalidOperationException("Received empty text response from Gemini API.");
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
