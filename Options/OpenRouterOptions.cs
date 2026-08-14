namespace GitReview.Options;

internal static class OpenRouterOptions
{
    public const string ApiKeyEnvironmentVariable = "OPENROUTER_API_KEY";
    public const string ModelEnvironmentVariable = "OPENROUTER_MODEL";
    public const string DefaultModel = "poolside/laguna-s-2.1:free";
    public const string Endpoint = "https://openrouter.ai/api/v1/chat/completions";

    public static string Model =>
        Environment.GetEnvironmentVariable(ModelEnvironmentVariable) is { Length: > 0 } m
            ? m
            : DefaultModel;
}