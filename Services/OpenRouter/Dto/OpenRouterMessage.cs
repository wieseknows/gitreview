using System.Text.Json.Serialization;

namespace GitReview.Services.OpenRouter.Dto;

internal sealed record OpenRouterMessage(
    [property: JsonPropertyName("role")] string Role,
    [property: JsonPropertyName("content")] string Content
);
