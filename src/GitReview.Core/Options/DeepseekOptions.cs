namespace GitReview.Core.Options;

public static class DeepSeekOptions
{
    public const string ApiKeyEnvironmentVariable = "DEEPSEEK_API_KEY";
    public const string ModelEnvironmentVariable = "DEEPSEEK_MODEL";
    public const string DefaultModel = "deepseek-chat";
    public const string Endpoint = "https://api.deepseek.com/v1/chat/completions";

    public static string Model =>
        Environment.GetEnvironmentVariable(ModelEnvironmentVariable) is { Length: > 0 } m
            ? m
            : DefaultModel;
}