using System.Text.Json.Serialization;

namespace GitReview.Services.OpenRouter.Dto;

internal sealed record OpenRouterRequest(
    [property: JsonPropertyName("model")] string Model,
    [property: JsonPropertyName("messages")] OpenRouterMessage[] Messages
);
