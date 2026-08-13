using System.Text.Json.Serialization;

namespace GitReview.Services.Gemini.Dto;

internal record GeminiContent(
    [property: JsonPropertyName("parts")] GeminiPart[]? Parts
);
