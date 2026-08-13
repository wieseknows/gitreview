using System.Text.Json.Serialization;

namespace GitReview.Services.Gemini.Dto;

internal record GeminiPart(
    [property: JsonPropertyName("text")] string Text
);