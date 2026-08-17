using GitReview.Core.Helpers;
using GitReview.Core.Models;
using GitReview.Shared.Enums;

namespace GitReview.Core.Strategies;

public sealed class RawDiffOutputStrategy : IOutputStrategy
{
    public ReviewExecutionMode Mode => ReviewExecutionMode.RawDiffOnly;

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