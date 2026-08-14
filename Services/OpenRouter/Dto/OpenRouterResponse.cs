using System.Text.Json.Serialization;

namespace GitReview.Services.OpenRouter.Dto;

internal sealed record OpenRouterResponse(
    [property: JsonPropertyName("choices")] OpenRouterChoice[]? Choices
);
