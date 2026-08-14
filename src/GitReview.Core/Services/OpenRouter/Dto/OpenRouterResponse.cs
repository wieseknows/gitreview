using System.Text.Json.Serialization;

namespace GitReview.Core.Services.OpenRouter.Dto;

public sealed record OpenRouterResponse(
    [property: JsonPropertyName("choices")] OpenRouterChoice[]? Choices
);
