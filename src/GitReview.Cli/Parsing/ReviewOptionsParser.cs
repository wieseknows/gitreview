using GitReview.Core.Models;
using GitReview.Shared.Constants;
using GitReview.Shared.Enums;
using GitReview.Shared.Providers;

namespace GitReview.Cli.Parsing
{
    internal sealed class ReviewOptionsParser
    {
        public static ReviewOptions Parse(string[] args)
        {
            string? rawProviderArg = GetProviderValue(args);

            ReviewExecutionMode? mode = null;
            AiProvider? selectedProvider = !string.IsNullOrWhiteSpace(rawProviderArg)
                ? ProviderMetadata.ParseProvider(rawProviderArg)
                : null;

            if (Has(args, "deepseek", "--deepseek", "-ds"))
            {
                mode = ReviewExecutionMode.AiReview;
                selectedProvider ??= AiProvider.DeepSeek;
            }
            else if (Has(args, "gemini", "--gemini", "-gem"))
            {
                mode = ReviewExecutionMode.AiReview;
                selectedProvider ??= AiProvider.Gemini;
            }
            else if (Has(args, "openrouter", "--openrouter", "-or"))
            {
                mode = ReviewExecutionMode.AiReview;
                selectedProvider ??= AiProvider.OpenRouter;
            }
            else if (Has(args, "ai", "--ai"))
            {
                mode = ReviewExecutionMode.AiReview;
            }
            else if (Has(args, "raw", "--raw", "-r"))
            {
                mode = ReviewExecutionMode.RawDiffOnly;
            }

            if (selectedProvider.HasValue && !mode.HasValue)
            {
                mode = ReviewExecutionMode.AiReview;
            }

            if (!selectedProvider.HasValue)
            {
                var envProvider = Environment.GetEnvironmentVariable(EnvVariables.Provider);
                selectedProvider = ProviderMetadata.ParseProvider(envProvider);
            }

            return new ReviewOptions(
                mode ?? ReviewExecutionMode.PromptWithClipboard,
                selectedProvider.Value);
        }

        private static bool Has(string[] a, params string[] keys)
        {
            return a.Any(x => keys.Any(k => x.Equals(k, StringComparison.OrdinalIgnoreCase)));
        }

        static string? GetProviderValue(string[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                var arg = a[i];
                if ((arg.Equals("--provider", StringComparison.OrdinalIgnoreCase) || arg.Equals("-p", StringComparison.OrdinalIgnoreCase))
                    && i + 1 < a.Length)
                {
                    return a[i + 1];
                }
                if (arg.StartsWith("--provider=", StringComparison.OrdinalIgnoreCase))
                {
                    return arg.Substring("--provider=".Length);
                }
            }
            return null;
        }
    }
}
