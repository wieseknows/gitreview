using GitReview.Core.Services;
using GitReview.Core.Services.DeepSeek;
using GitReview.Core.Services.Gemini;
using GitReview.Core.Services.OpenRouter;
using GitReview.Shared.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace GitReview.Cli.Composition;

internal static class LlmServiceCollectionExtensions
{
    private const int LlmTimeoutInMinutes = 3;

    public static IServiceCollection AddLlmReviewService(this IServiceCollection services, AiProvider provider)
    {
        return provider switch
        {
            AiProvider.Gemini => services.AddGemini(),
            AiProvider.DeepSeek => services.AddDeepSeek(),
            AiProvider.OpenRouter => services.AddOpenRouter(),
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, $"Unsupported LLM provider: {provider}")
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