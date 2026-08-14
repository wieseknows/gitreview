using System.Text.Json.Serialization;

namespace GitReview.Core.Services.OpenRouter.Dto;

public sealed record OpenRouterMessage(
    [property: JsonPropertyName("role")] string Role,
    [property: JsonPropertyName("content")] string Content
);
