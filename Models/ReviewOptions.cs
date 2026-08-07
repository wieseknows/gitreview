namespace GitReview.Models;

public enum OutputMode
{
    PromptWithClipboard,
    RawDiffOnly
}

public class ReviewOptions
{
    public OutputMode Mode { get; init; } = OutputMode.PromptWithClipboard;

    public static ReviewOptions Parse(string[] args)
    {
        var isRaw = args.Any(arg => arg.Equals(
            "raw", StringComparison.OrdinalIgnoreCase)
            || arg.Equals("--raw", StringComparison.OrdinalIgnoreCase)
            || arg.Equals("-r", StringComparison.OrdinalIgnoreCase));

        return new ReviewOptions
        {
            Mode = isRaw
                ? OutputMode.RawDiffOnly
                : OutputMode.PromptWithClipboard,
        };
    }
}