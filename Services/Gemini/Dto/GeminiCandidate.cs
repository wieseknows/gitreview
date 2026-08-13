using System.Text.Json.Serialization;

namespace GitReview.Services.Gemini.Dto;

internal record GeminiCandidate(
    [property: JsonPropertyName("content")] GeminiContent? Content
);
