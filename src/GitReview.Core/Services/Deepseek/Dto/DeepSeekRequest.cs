using System.Text.Json.Serialization;

namespace GitReview.Core.Services.Deepseek.Dto;

public sealed record DeepSeekRequest(
    [property: JsonPropertyName("model")] string Model,
    [property: JsonPropertyName("messages")] DeepSeekMessage[] Messages
);
