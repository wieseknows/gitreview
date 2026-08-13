using System.Text.Json.Serialization;

namespace GitReview.Services.Gemini.Dto;

internal record GeminiResponse(
    [property: JsonPropertyName("candidates")] GeminiCandidate[]? Candidates
);
