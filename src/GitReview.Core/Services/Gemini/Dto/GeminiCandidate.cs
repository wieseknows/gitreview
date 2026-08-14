using System.Text.Json.Serialization;

namespace GitReview.Core.Services.Gemini.Dto;

public record GeminiCandidate(
    [property: JsonPropertyName("content")] GeminiContent? Content
);
