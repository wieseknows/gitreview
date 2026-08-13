namespace GitReview.Options;

internal static class DeepSeekOptions
{
    public const string ApiKeyEnvironmentVariable = "DEEPSEEK_API_KEY";
    public const string ModelEnvironmentVariable = "DEEPSEEK_MODEL";
    public const string DefaultModel = "deepseek-chat";
    public const string BaseUrl = "https://api.deepseek.com/v1/";

    public static string Model =>
        Environment.GetEnvironmentVariable(ModelEnvironmentVariable) is { Length: > 0 } m
            ? m
            : DefaultModel;
}