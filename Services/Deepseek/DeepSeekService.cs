using GitReview.Options;
using GitReview.Services.Deepseek.Dto;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GitReview.Services.DeepSeek;

internal sealed class DeepSeekService : ILlmReviewService
{
    private readonly HttpClient _httpClient;

    public DeepSeekService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(DeepSeekOptions.BaseUrl);
    }

    public async Task<string> GetReviewAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var apiKey = Environment.GetEnvironmentVariable(DeepSeekOptions.ApiKeyEnvironmentVariable);

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                $"API Key is missing. Please set the '{DeepSeekOptions.ApiKeyEnvironmentVariable}' environment variable.");
        }

        Console.WriteLine($"📡 Model: {DeepSeekOptions.Model} (DeepSeek)");

        using var request = new HttpRequestMessage(HttpMethod.Post, "chat/completions");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var requestBody = new DeepSeekRequest(
            Model: DeepSeekOptions.Model,
            Messages: [
                new DeepSeekMessage("system", "You are an expert Senior Code Reviewer."),
                new DeepSeekMessage("user", prompt)
            ]
        );

        request.Content = JsonContent.Create(requestBody);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorPayload = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"DeepSeek API error [{response.StatusCode}]: {errorPayload}");
        }

        var result = await response.Content.ReadFromJsonAsync<DeepSeekResponse>(cancellationToken: cancellationToken);
        var textResult = result?.Choices?
            .Select(c => c.Message?.Content)
            .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));

        if (string.IsNullOrWhiteSpace(textResult))
        {
            throw new InvalidOperationException("Received empty text response from DeepSeek API.");
        }

        return textResult;
    }
}