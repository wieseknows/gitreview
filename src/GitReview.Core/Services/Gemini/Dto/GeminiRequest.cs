using System.Text.Json.Serialization;

namespace GitReview.Core.Services.Gemini.Dto;

public record GeminiRequest(
    [property: JsonPropertyName("contents")] GeminiContent[] Contents
);
