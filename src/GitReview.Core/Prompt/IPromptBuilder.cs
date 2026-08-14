using GitReview.Core.Models;

namespace GitReview.Core.Prompt;

public interface IPromptBuilder
{
    string Build(GitDiffResult diff, string repository, string branch);
}