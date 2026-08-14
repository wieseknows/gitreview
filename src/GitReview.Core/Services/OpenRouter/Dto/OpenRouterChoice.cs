using System.Text.Json.Serialization;

namespace GitReview.Core.Services.OpenRouter.Dto;

public sealed record OpenRouterChoice(
    [property: JsonPropertyName("message")] OpenRouterMessage? Message
);