using GitReview.Shared.Enums;
using GitReview.Shared.Providers;

public static class DeepSeekOptions
{
    public static readonly string ApiKeyEnvironmentVariable = AiProvider.DeepSeek.GetApiKeyEnvVar();
    public static readonly string ModelEnvironmentVariable = AiProvider.DeepSeek.GetModelEnvVar();
    public const string Endpoint = "https://api.deepseek.com/v1/chat/completions";

    public static string Model =>
        Environment.GetEnvironmentVariable(ModelEnvironmentVariable) is { Length: > 0 } m
            ? m
            : AiProvider.DeepSeek.GetDefaultModel();
}