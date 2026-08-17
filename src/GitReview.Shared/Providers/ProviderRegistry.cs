using GitReview.Shared.Constants;
using GitReview.Shared.Enums;
using System;
using System.Collections.Generic;

namespace GitReview.Shared.Providers
{
    public static class ProviderRegistry
    {
        public const AiProvider DefaultProvider = AiProvider.OpenRouter;

        private static readonly IReadOnlyDictionary<AiProvider, ProviderSpec> Specs =
            new Dictionary<AiProvider, ProviderSpec>
            {
                [AiProvider.OpenRouter] = new(
                    AiProvider.OpenRouter,
                    EnvVariables.OpenRouterApiKey,
                    EnvVariables.OpenRouterModelKey,
                    [
                        "poolside/laguna-s-2.1:free",
                        "nvidia/nemotron-3-super:free",
                        "cohere/north-mini-code:free",
                        "deepseek/deepseek-r1:free"
                    ]),

                [AiProvider.Gemini] = new(
                    AiProvider.Gemini,
                    EnvVariables.GeminiApiKey,
                    EnvVariables.GeminiModelKey,
                    [
                        "gemini-2.0-flash",
                        "gemini-1.5-flash",
                        "gemini-1.5-pro"
                    ]),

                [AiProvider.DeepSeek] = new(
                    AiProvider.DeepSeek,
                    EnvVariables.DeepSeekApiKey,
                    EnvVariables.DeepSeekModelKey,
                    [
                        "deepseek-chat",
                        "deepseek-reasoner"
                    ])
            };

        public static ProviderSpec GetSpec(this AiProvider provider)
        {
            if (Specs.TryGetValue(provider, out var spec))
            {
                return spec;
            }

            throw new ArgumentOutOfRangeException(nameof(provider), provider, $"Provider '{provider}' is not registered.");
        }

        public static AiProvider ParseProvider(string? provider, AiProvider fallback = DefaultProvider)
        {
            if (string.IsNullOrWhiteSpace(provider))
            {
                return fallback;
            }

            return provider!.Trim().ToLowerInvariant() switch
            {
                "gemini" or "google" => AiProvider.Gemini,
                "deepseek" or "ds" => AiProvider.DeepSeek,
                "openrouter" or "or" => AiProvider.OpenRouter,
                _ => fallback
            };
        }

        public static string GetApiKey(this AiProvider provider) => provider.GetSpec().GetApiKey();
        public static string GetApiKeyEnvVar(this AiProvider provider) => provider.GetSpec().ApiKeyEnvVar;
        public static IReadOnlyList<string> GetAvailableModels(this AiProvider provider) => provider.GetSpec().Models;
        public static string GetModel(this AiProvider provider) => provider.GetSpec().GetModel();
        public static string ToCliValue(this AiProvider provider) => provider.ToString().ToLowerInvariant();
    }
}