using System.Text.Json.Serialization;

namespace GitReview.Services.Deepseek.Dto;

internal sealed record DeepSeekChoice(
    [property: JsonPropertyName("message")] DeepSeekMessage? Message
);