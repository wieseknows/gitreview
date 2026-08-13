using GitReview.Models;

namespace GitReview.Prompt;

internal interface IPromptBuilder
{
    string Build(GitDiffResult diff, string repository, string branch);
}