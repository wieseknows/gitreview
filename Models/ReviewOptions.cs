namespace GitReview.Models;

internal enum OutputMode
{
    PromptWithClipboard,
    RawDiffOnly,
    AiReview
}

internal sealed class ReviewOptions
{
    public OutputMode Mode { get; init; } = OutputMode.PromptWithClipboard;

    private ReviewOptions(OutputMode mode)
    {
        Mode = mode;
    }

    public static ReviewOptions Parse(string[] args)
    {
        static bool Has(string[] a, params string[] keys) =>
            a.Any(x => keys.Any(k => x.Equals(k, StringComparison.OrdinalIgnoreCase)));

        if (Has(args, "ai", "--ai"))
        {
            return new(OutputMode.AiReview);
        }

        if (Has(args, "raw", "--raw", "-r"))
        {
            return new(OutputMode.RawDiffOnly);
        }

        return new(OutputMode.PromptWithClipboard);
    }
}