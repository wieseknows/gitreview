using System.Text.Json.Serialization;

namespace GitReview.Core.Services.Deepseek.Dto;

public sealed record DeepSeekMessage(
    [property: JsonPropertyName("role")] string Role,
    [property: JsonPropertyName("content")] string Content
);
