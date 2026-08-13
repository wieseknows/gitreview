using GitReview.Models;

internal interface IOutputStrategy
{
    OutputMode Mode { get; }
    Task ProcessAsync(GitDiffResult diff, CancellationToken cancellationToken = default);
}