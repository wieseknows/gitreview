using GitReview.Core.Models;

namespace GitReview.Core.Git;

public interface IGitService
{
    GitDiffResult GetDiff();
    string GetRepositoryRoot();
    string GetCurrentBranch();
    bool IsGitRepository();
}