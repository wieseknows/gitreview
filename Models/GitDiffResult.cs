namespace GitReview.Models;

public class GitDiffResult
{
    public string StagedDiff { get; init; } = string.Empty;
    public string WorkingTreeDiff { get; init; } = string.Empty;

    public string CombinedDiff =>
        $"""
        {StagedDiff}

        {WorkingTreeDiff}
        """;

    public bool HasChanges =>
        !string.IsNullOrWhiteSpace(StagedDiff)
        || !string.IsNullOrWhiteSpace(WorkingTreeDiff);

    private string[]? _lines;

    private string[] Lines =>
        _lines ??= CombinedDiff.Split(
            ['\r', '\n'],
            StringSplitOptions.RemoveEmptyEntries);

    public int ChangedFiles =>
        Lines.Count(x => x.StartsWith("diff --git", StringComparison.Ordinal));

    public int ChangedLines =>
        Lines.Count(x =>
            (x.StartsWith('+') || x.StartsWith('-'))
            && !x.StartsWith("+++")
            && !x.StartsWith("---"));
}