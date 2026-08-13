namespace GitReview.Options;

internal static class GeminiOptions
{
    public const string ApiKeyEnvironmentVariable = "GEMINI_API_KEY";
    public const string ModelEnvironmentVariable = "GEMINI_MODEL";
    public const string DefaultModel = "gemini-3.6-flash";
    public const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta";

    public static string Model =>
        Environment.GetEnvironmentVariable(ModelEnvironmentVariable) is { Length: > 0 } m
            ? m
            : DefaultModel;

    public static string GetGenerateContentUrl(string? model = null)
     => $"{BaseUrl}/models/{model ?? Model}:generateContent";
}