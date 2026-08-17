using GitReview.Shared.Enums;
using GitReview.Shared.Providers;

namespace GitReview.Core.Options;

public static class GeminiOptions
{
    public static readonly string ApiKeyEnvironmentVariable = AiProvider.Gemini.GetApiKeyEnvVar();
    public static readonly string ModelEnvironmentVariable = AiProvider.Gemini.GetModelEnvVar();
    public const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta";

    public static string Model =>
        Environment.GetEnvironmentVariable(ModelEnvironmentVariable) is { Length: > 0 } m
            ? m
            : AiProvider.Gemini.GetDefaultModel();

    public static string GetGenerateContentUrl(string? model = null)
        => $"{BaseUrl}/models/{model ?? Model}:generateContent";
}