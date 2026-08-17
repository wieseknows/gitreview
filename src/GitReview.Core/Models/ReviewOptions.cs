using GitReview.Shared.Constants;
using GitReview.Shared.Enums;

namespace GitReview.Core.Models;

public sealed class ReviewOptions
{
    private const string DefaultLlmProvider = "openrouter";

    public ReviewExecutionMode Mode { get; init; } = ReviewExecutionMode.PromptWithClipboard;
    public string Provider { get; init; } = DefaultLlmProvider;

    public static ReviewOptions Parse(string[] args)
    {
        static bool Has(string[] a, params string[] keys) =>
            a.Any(x => keys.Any(k => x.Equals(k, StringComparison.OrdinalIgnoreCase)));

        static string? GetProviderValue(string[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                var arg = a[i];
                if ((arg.Equals("--provider", StringComparison.OrdinalIgnoreCase)
                    || arg.Equals("-p", StringComparison.OrdinalIgnoreCase)) && i + 1 < a.Length)
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

        var mode = ReviewExecutionMode.PromptWithClipboard;
        string? provider = GetProviderValue(args);

        if (Has(args, "deepseek", "--deepseek", "-ds"))
        {
            mode = ReviewExecutionMode.AiReview;
            provider ??= "deepseek";
        }
        else if (Has(args, "gemini", "--gemini", "-gem"))
        {
            mode = ReviewExecutionMode.AiReview;
            provider ??= "gemini";
        }
        else if (Has(args, "openrouter", "--openrouter", "-or"))
        {
            mode = ReviewExecutionMode.AiReview;
            provider ??= "openrouter";
        }
        else if (Has(args, "ai", "--ai"))
        {
            mode = ReviewExecutionMode.AiReview;
        }
        else if (Has(args, "raw", "--raw", "-r"))
        {
            mode = ReviewExecutionMode.RawDiffOnly;
        }

        if (provider != null && mode == ReviewExecutionMode.PromptWithClipboard)
        {
            mode = ReviewExecutionMode.AiReview;
        }

        provider ??= Environment.GetEnvironmentVariable(EnvVariables.Provider);
        if (string.IsNullOrWhiteSpace(provider))
        {
            provider = DefaultLlmProvider;
        }

        return new ReviewOptions
        {
            Mode = mode,
            Provider = provider.ToLowerInvariant()
        };
    }
}