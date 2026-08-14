using System.Text.Json.Serialization;

namespace GitReview.Core.Services.OpenRouter.Dto;

public sealed record OpenRouterRequest(
    [property: JsonPropertyName("model")] string Model,
    [property: JsonPropertyName("messages")] OpenRouterMessage[] Messages
);
