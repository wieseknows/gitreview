using GitReview.Core.Services.Deepseek.Dto;
using GitReview.Shared.Enums;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GitReview.Core.Services.DeepSeek;

public sealed class DeepSeekService : BaseLlmService
{
    protected override AiProvider Provider => AiProvider.DeepSeek;

    public DeepSeekService(HttpClient httpClient) : base(httpClient) { }

    protected override HttpRequestMessage CreateRequest(string endpoint, string model, string apiKey, string prompt)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        request.Content = JsonContent.Create(new DeepSeekRequest(
            Model: model,
            Messages:
            [
                new DeepSeekMessage("system", "You are an expert Senior Code Reviewer."),
                new DeepSeekMessage("user", prompt)
            ]
        ));

        return request;
    }

    protected override async Task<string?> ExtractTextResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var result = await response.Content.ReadFromJsonAsync<DeepSeekResponse>(cancellationToken);
        return result?.Choices?
            .Select(c => c.Message?.Content)
            .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));
    }
}
