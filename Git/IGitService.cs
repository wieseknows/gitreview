using GitReview.Models;

namespace GitReview.Git;

internal interface IGitService
{
    GitDiffResult GetDiff();
    string GetRepositoryRoot();
    string GetCurrentBranch();
    bool IsGitRepository();
}