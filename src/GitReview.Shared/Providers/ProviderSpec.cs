using GitReview.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GitReview.Shared.Providers
{
    public sealed class ProviderSpec
    {
        public AiProvider Provider { get; }
        public string ApiKeyEnvVar { get; }
        public string ModelEnvVar { get; }
        public IReadOnlyList<string> Models { get; }

        public ProviderSpec(
            AiProvider provider,
            string apiKeyEnvVar,
            string modelEnvVar,
            IReadOnlyList<string> models)
        {
            Provider = provider;
            ApiKeyEnvVar = apiKeyEnvVar;
            ModelEnvVar = modelEnvVar;
            Models = models;
        }

        public string DefaultModel => Models.FirstOrDefault() ?? string.Empty;

        public string GetModel()
        {
            var value = Environment.GetEnvironmentVariable(ModelEnvVar);
            return string.IsNullOrWhiteSpace(value) ? DefaultModel : value;
        }

        public string GetApiKey()
        {
            var key = Environment.GetEnvironmentVariable(ApiKeyEnvVar);
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException(
                    $"API Key is missing. Please set the '{ApiKeyEnvVar}' environment variable.");
            }
            return key;
        }
    }
}