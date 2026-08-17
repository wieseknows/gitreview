using GitReview.Core.Models;
using GitReview.Shared.Enums;

public interface IOutputStrategy
{
    ReviewExecutionMode Mode { get; }
    Task ProcessAsync(GitDiffResult diff, CancellationToken cancellationToken = default);
}