using System.Text.Json.Serialization;

namespace GitReview.Services.Deepseek.Dto;

internal sealed record DeepSeekResponse(
    [property: JsonPropertyName("choices")] DeepSeekChoice[]? Choices
);
