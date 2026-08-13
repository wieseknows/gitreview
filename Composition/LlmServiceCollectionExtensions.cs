using GitReview.Services;
using GitReview.Services.DeepSeek;
using GitReview.Services.Gemini;
using Microsoft.Extensions.DependencyInjection;

namespace GitReview.Composition;

internal static class LlmServiceCollectionExtensions
{
    private static readonly int LlmTimeoutInMinutes = 3;

    public static IServiceCollection AddLlmReviewService(this IServiceCollection services)
    {
        var provider = Environment.GetEnvironmentVariable("GIT_REVIEW_PROVIDER")
            ?? "gemini";

        return provider.ToLowerInvariant() switch
        {
            "gemini" => services.AddGeminiLlm(),
            "deepseek" => services.AddDeepSeekLlm(),
            _ => throw new InvalidOperationException(
                $"Unknown GIT_REVIEW_PROVIDER '{provider}'. Supported: gemini, deepseek")
        };
    }

    private static IServiceCollection AddGeminiLlm(this IServiceCollection services)
    {
        services.AddHttpClient<ILlmReviewService, GeminiService>(client =>
        {
            client.Timeout = TimeSpan.FromMinutes(LlmTimeoutInMinutes);
        });
        return services;
    }

    private static IServiceCollection AddDeepSeekLlm(this IServiceCollection services)
    {
        services.AddHttpClient<ILlmReviewService, DeepSeekService>(client =>
        {
            client.Timeout = TimeSpan.FromMinutes(LlmTimeoutInMinutes);
        });
        return services;
    }
}