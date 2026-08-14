using System.Text.Json.Serialization;

namespace GitReview.Core.Services.Deepseek.Dto;

public sealed record DeepSeekResponse(
    [property: JsonPropertyName("choices")] DeepSeekChoice[]? Choices
);
