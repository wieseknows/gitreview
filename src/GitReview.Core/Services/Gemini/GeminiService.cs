using GitReview.Core.Services.Gemini.Dto;
using GitReview.Shared.Enums;
using System.Net.Http.Json;

namespace GitReview.Core.Services.Gemini;

public sealed class GeminiService : BaseLlmService
{
    protected override AiProvider Provider => AiProvider.Gemini;

    public GeminiService(HttpClient httpClient) : base(httpClient) { }

    protected override HttpRequestMessage CreateRequest(string endpoint, string model, string apiKey, string prompt)
    {
        var fullUrl = $"{endpoint}/models/{model}:generateContent";

        var request = new HttpRequestMessage(HttpMethod.Post, fullUrl);
        request.Headers.TryAddWithoutValidation("x-goog-api-key", apiKey);

        request.Content = JsonContent.Create(new GeminiRequest(
        [
            new GeminiContent(
            [
                new GeminiPart(prompt)
            ])
        ]));

        return request;
    }

    protected override async Task<string?> ExtractTextResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var result = await response.Content.ReadFromJsonAsync<GeminiResponse>(cancellationToken);
        return result?.Candidates?
            .SelectMany(c => c.Content?.Parts ?? [])
            .Select(p => p.Text)
            .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));
    }
}
