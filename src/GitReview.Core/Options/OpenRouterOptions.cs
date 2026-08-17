using GitReview.Shared.Constants;
using GitReview.Shared.Enums;
using GitReview.Shared.Providers;

namespace GitReview.Core.Options;

public static class OpenRouterOptions
{
    public const string ApiKeyEnvironmentVariable = EnvVariables.OpenRouterApiKey;
    public const string ModelEnvironmentVariable = EnvVariables.OpenRouterModelKey;
    public const string Endpoint = "https://openrouter.ai/api/v1/chat/completions";

    public static string Model =>
        Environment.GetEnvironmentVariable(ModelEnvironmentVariable) is { Length: > 0 } m
            ? m
            : AiProvider.OpenRouter.GetDefaultModel();
}