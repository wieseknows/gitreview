using System.Text.Json.Serialization;

namespace GitReview.Services.Deepseek.Dto;

internal sealed record DeepSeekRequest(
    [property: JsonPropertyName("model")] string Model,
    [property: JsonPropertyName("messages")] DeepSeekMessage[] Messages
);
