using System.Text.Json.Serialization;

namespace GitReview.Services.OpenRouter.Dto;

internal sealed record OpenRouterChoice(
    [property: JsonPropertyName("message")] OpenRouterMessage? Message
);