using System.Text.Json.Serialization;

namespace GitReview.Core.Services.Gemini.Dto;

public record GeminiContent(
    [property: JsonPropertyName("parts")] GeminiPart[]? Parts
);
