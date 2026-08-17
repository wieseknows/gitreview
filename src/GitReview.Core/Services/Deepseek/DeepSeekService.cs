using GitReview.Core.Services.Deepseek.Dto;
using GitReview.Shared.Enums;
using GitReview.Shared.Providers;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GitReview.Core.Services.DeepSeek;

public sealed class DeepSeekService : ILlmReviewService
{
    private const string Endpoint = "https://api.deepseek.com/v1/chat/completions";

    private readonly HttpClient _httpClient;

    public DeepSeekService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetReviewAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var apiKey = Environment.GetEnvironmentVariable(AiProvider.DeepSeek.GetApiKeyEnvVar());
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                $"API Key is missing. Please set the '{AiProvider.DeepSeek.GetApiKeyEnvVar()}' environment variable.");
        }

        var model = AiProvider.DeepSeek.GetModel();
        var requestBody = new DeepSeekRequest(
            Model: model,
            Messages:
            [
                new DeepSeekMessage("system", "You are an expert Senior Code Reviewer."),
                new DeepSeekMessage("user", prompt)
            ]
        );

        Console.WriteLine($"📡 Model: {model} (DeepSeek)");

        using var request = new HttpRequestMessage(HttpMethod.Post, Endpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
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