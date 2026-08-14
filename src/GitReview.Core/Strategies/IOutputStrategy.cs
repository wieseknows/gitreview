using GitReview.Core.Models;

public interface IOutputStrategy
{
    OutputMode Mode { get; }
    Task ProcessAsync(GitDiffResult diff, CancellationToken cancellationToken = default);
}