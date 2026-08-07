using GitReview.Models;

namespace GitReview.Git;

public interface IGitService
{
    GitDiffResult GetDiff();
    string GetRepositoryRoot();
    string GetCurrentBranch();
    bool IsGitRepository();
}