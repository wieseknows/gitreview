using GitReview.Core.Options;
using GitReview.Core.Services.Gemini.Dto;
using System.Net.Http.Json;

namespace GitReview.Core.Services.Gemini;

public sealed class GeminiService : ILlmReviewService
{
    private readonly HttpClient _httpClient;

    public GeminiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetReviewAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var apiKey = Environment.GetEnvironmentVariable(GeminiOptions.ApiKeyEnvironmentVariable);

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                $"API Key is missing. Please set the '{GeminiOptions.ApiKeyEnvironmentVariable}' environment variable.");
        }

        var requestUri = GeminiOptions.GetGenerateContentUrl();
        var requestBody = new GeminiRequest(
            [new GeminiContent([new GeminiPart(prompt)])]
        );

        Console.WriteLine($"📡 Model: {GeminiOptions.Model}");

        using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
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
}
