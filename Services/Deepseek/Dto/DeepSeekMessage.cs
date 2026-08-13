using System.Text.Json.Serialization;

namespace GitReview.Services.Deepseek.Dto;

internal sealed record DeepSeekMessage(
    [property: JsonPropertyName("role")] string Role,
    [property: JsonPropertyName("content")] string Content
);
