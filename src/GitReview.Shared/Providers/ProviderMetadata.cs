using GitReview.Shared.Constants;
using GitReview.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GitReview.Shared.Providers
{
    public static class ProviderMetadata
    {
        public const AiProvider DefaultProvider = AiProvider.OpenRouter;

        public static string GetApiKeyEnvVar(this AiProvider provider) => provider switch
        {
            AiProvider.Gemini => EnvVariables.GeminiApiKey,
            AiProvider.DeepSeek => EnvVariables.DeepSeekApiKey,
            AiProvider.OpenRouter => EnvVariables.OpenRouterApiKey,
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, null)
        };

        public static string GetModelEnvVar(this AiProvider provider) => provider switch
        {
            AiProvider.Gemini => EnvVariables.GeminiModelKey,
            AiProvider.DeepSeek => EnvVariables.DeepSeekModelKey,
            AiProvider.OpenRouter => EnvVariables.OpenRouterModelKey,
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, null)
        };

        public static IReadOnlyList<string> GetAvailableModels(this AiProvider provider) => provider switch
        {
            AiProvider.OpenRouter => new[]
            {
                "poolside/laguna-s-2.1:free",
                "nvidia/nemotron-3-super:free",
                "cohere/north-mini-code:free",
                "deepseek/deepseek-r1:free"
            },
            AiProvider.Gemini => new[]
            {
                "gemini-2.0-flash",
                "gemini-1.5-flash",
                "gemini-1.5-pro"
            },
            AiProvider.DeepSeek => new[]
            {
                "deepseek-chat",
                "deepseek-reasoner"
            },
            _ => []
        };

        public static string GetDefaultModel(this AiProvider provider)
        {
            return provider.GetAvailableModels().FirstOrDefault() ?? string.Empty;
        }

        public static AiProvider ParseProvider(string? provider) => provider?.Trim().ToLowerInvariant() switch
        {
            "gemini" or "google" => AiProvider.Gemini,
            "deepseek" or "ds" => AiProvider.DeepSeek,
            "openrouter" or "or" => AiProvider.OpenRouter,
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, null)
        };

        public static string ToCliValue(this AiProvider provider) => provider switch
        {
            AiProvider.Gemini => "gemini",
            AiProvider.DeepSeek => "deepseek",
            AiProvider.OpenRouter => "openrouter",
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, null)
        };
    }
}