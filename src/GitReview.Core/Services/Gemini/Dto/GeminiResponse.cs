using System.Text.Json.Serialization;

namespace GitReview.Core.Services.Gemini.Dto;

public record GeminiResponse(
    [property: JsonPropertyName("candidates")] GeminiCandidate[]? Candidates
);
