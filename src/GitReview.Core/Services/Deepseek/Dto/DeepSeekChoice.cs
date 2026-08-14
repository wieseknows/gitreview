using System.Text.Json.Serialization;

namespace GitReview.Core.Services.Deepseek.Dto;

public sealed record DeepSeekChoice(
    [property: JsonPropertyName("message")] DeepSeekMessage? Message
);