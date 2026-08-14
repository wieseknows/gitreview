namespace GitReview.Models;

internal enum OutputMode
{
    PromptWithClipboard,
    RawDiffOnly,
    AiReview
}

internal sealed class ReviewOptions
{
    private const string DefaultLlmProvider = "gemini";

    public OutputMode Mode { get; init; } = OutputMode.PromptWithClipboard;
    public string Provider { get; init; } = "gemini";

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

        var mode = OutputMode.PromptWithClipboard;
        string? provider = GetProviderValue(args);

        if (Has(args, "deepseek", "--deepseek", "-ds"))
        {
            mode = OutputMode.AiReview;
            provider ??= "deepseek";
        }
        else if (Has(args, "gemini", "--gemini", "-gem"))
        {
            mode = OutputMode.AiReview;
            provider ??= "gemini";
        }
        else if (Has(args, "openrouter", "--openrouter", "-or"))
        {
            mode = OutputMode.AiReview;
            provider ??= "openrouter";
        }
        else if (Has(args, "ai", "--ai"))
        {
            mode = OutputMode.AiReview;
        }
        else if (Has(args, "raw", "--raw", "-r"))
        {
            mode = OutputMode.RawDiffOnly;
        }

        if (provider != null && mode == OutputMode.PromptWithClipboard)
        {
            mode = OutputMode.AiReview;
        }

        provider ??= Environment.GetEnvironmentVariable("GIT_REVIEW_PROVIDER");
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