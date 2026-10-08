using GitReview.Core.Models;

namespace GitReview.Core.Git;

public interface IGitService
{
    GitDiffResult GetDiff(string? fromBranch = null, string? toBranch = null);
    string GetRepositoryRoot();
    string GetCurrentBranch();
    bool IsGitRepository();
    void ApplyPatch(string patchContent);
}