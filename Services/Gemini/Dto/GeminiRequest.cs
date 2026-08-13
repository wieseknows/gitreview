using System.Text.Json.Serialization;

namespace GitReview.Services.Gemini.Dto;

internal record GeminiRequest(
    [property: JsonPropertyName("contents")] GeminiContent[] Contents
);
