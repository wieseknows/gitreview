using GitReview.Models;

namespace GitReview.Prompt;

public interface IPromptBuilder
{
    string Build(GitDiffResult diff, string repository, string branch);
}