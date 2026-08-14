using System.Text.Json.Serialization;

namespace GitReview.Core.Services.Gemini.Dto;

public record GeminiPart(
    [property: JsonPropertyName("text")] string Text
);