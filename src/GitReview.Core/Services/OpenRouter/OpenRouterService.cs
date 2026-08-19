using GitReview.Core.Services.OpenRouter.Dto;
using GitReview.Shared.Enums;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GitReview.Core.Services.OpenRouter;

public sealed class OpenRouterService : BaseLlmService
{
    protected override AiProvider Provider => AiProvider.OpenRouter;

    public OpenRouterService(HttpClient httpClient) : base(httpClient) { }

    protected override HttpRequestMessage CreateRequest(string endpoint, string model, string apiKey, string prompt)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        request.Content = JsonContent.Create(new OpenRouterRequest(
            Model: model,
            Messages:
            [
                new OpenRouterMessage("system", "You are an expert Senior Code Reviewer."),
                new OpenRouterMessage("user", prompt)
            ]
        ));

        return request;
    }

    protected override async Task<string?> ExtractTextResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var result = await response.Content.ReadFromJsonAsync<OpenRouterResponse>(cancellationToken);
        return result?.Choices?
            .Select(c => c.Message?.Content)
            .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));
    }
}
