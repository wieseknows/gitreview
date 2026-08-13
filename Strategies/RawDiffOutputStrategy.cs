using GitReview.Helpers;
using GitReview.Models;

namespace GitReview.Strategies;

internal sealed class RawDiffOutputStrategy : IOutputStrategy
{
    public OutputMode Mode => OutputMode.RawDiffOnly;

    private const string FileName = "git_changes.diff";

    public async Task ProcessAsync(GitDiffResult diff, CancellationToken cancellationToken = default)
    {
        await ReviewOutputHelper.SaveClipboardAndRevealAsync(
            content: diff.CombinedDiff,
            fileName: FileName,
            successLabel: "Changes",
            cancellationToken: cancellationToken);
    }
}