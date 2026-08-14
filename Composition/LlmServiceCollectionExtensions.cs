using GitReview.Services;
using GitReview.Services.DeepSeek;
using GitReview.Services.Gemini;
using GitReview.Services.OpenRouter;
using Microsoft.Extensions.DependencyInjection;

namespace GitReview.Composition;

internal static class LlmServiceCollectionExtensions
{
    private static readonly int LlmTimeoutInMinutes = 3;

    public static IServiceCollection AddLlmReviewService(this IServiceCollection services, string provider)
    {
        return provider.ToLowerInvariant() switch
        {
            "gemini" => services.AddGemini(),
            "deepseek" => services.AddDeepSeek(),
            "openrouter" => services.AddOpenRouter(),
            _ => throw new InvalidOperationException(
                $"Unknown LLM provider '{provider}'. Supported providers: gemini, deepseek, openrouter")
        };
    }

    private static IServiceCollection AddGemini(this IServiceCollection services)
    {
        services.AddHttpClient<ILlmReviewService, GeminiService>(client =>
        {
            client.Timeout = TimeSpan.FromMinutes(LlmTimeoutInMinutes);
        });
        return services;
    }

    private static IServiceCollection AddDeepSeek(this IServiceCollection services)
    {
        services.AddHttpClient<ILlmReviewService, DeepSeekService>(client =>
        {
            client.Timeout = TimeSpan.FromMinutes(LlmTimeoutInMinutes);
        });
        return services;
    }

    private static IServiceCollection AddOpenRouter(this IServiceCollection services)
    {
        services.AddHttpClient<ILlmReviewService, OpenRouterService>(client =>
        {
            client.Timeout = TimeSpan.FromMinutes(LlmTimeoutInMinutes);
        });
        return services;
    }
}