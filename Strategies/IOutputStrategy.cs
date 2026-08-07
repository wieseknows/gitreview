using GitReview.Models;

namespace GitReview.Strategies;

public interface IOutputStrategy
{
    OutputMode Mode { get; }
    Task ProcessAsync(GitDiffResult diff);
}