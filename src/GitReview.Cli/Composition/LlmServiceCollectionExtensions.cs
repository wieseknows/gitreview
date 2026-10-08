using GitReview.Core.Services;
using GitReview.Core.Services.DeepSeek;
using GitReview.Core.Services.Gemini;
using GitReview.Core.Services.OpenRouter;
using GitReview.Core.Services.SambaNova;
using GitReview.Shared.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace GitReview.Cli.Composition;

internal static class LlmServiceCollectionExtensions
{
    private const int LlmTimeoutInMinutes = 3;

    public static IServiceCollection AddLlmReviewService(this IServiceCollection services, AiProvider provider)
    {
        services
            .AddHttpClient<ILlmReviewService>(client =>
            {
                client.Timeout = TimeSpan.FromMinutes(LlmTimeoutInMinutes);
            })
            .AddTypedClient<ILlmReviewService>((httpClient, sp) => provider switch
            {
                AiProvider.Gemini => new GeminiService(httpClient),
                AiProvider.DeepSeek => new DeepSeekService(httpClient),
                AiProvider.OpenRouter => new OpenRouterService(httpClient),
                AiProvider.SambaNova => new SambaNovaService(httpClient),
                _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, $"Unsupported LLM provider: {provider}")
            });

        return services;
    }
}
