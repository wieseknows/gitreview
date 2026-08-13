using GitReview.Git;
using GitReview.Helpers;
using GitReview.Models;
using GitReview.Prompt;

namespace GitReview.Strategies;

internal sealed class PromptOutputStrategy : IOutputStrategy
{
    private readonly IPromptBuilder _promptBuilder;
    private readonly IGitService _gitService;

    public OutputMode Mode => OutputMode.PromptWithClipboard;

    private const string FileName = "review.md";

    public PromptOutputStrategy(IPromptBuilder promptBuilder, IGitService gitService)
    {
        _promptBuilder = promptBuilder;
        _gitService = gitService;
    }

    public async Task ProcessAsync(GitDiffResult diff, CancellationToken cancellationToken = default)
    {
        var prompt = _promptBuilder.Build(
            diff,
            _gitService.GetRepositoryRoot(),
            _gitService.GetCurrentBranch());

        await ReviewOutputHelper.SaveClipboardAndRevealAsync(
            content: prompt,
            fileName: FileName,
            successLabel: "Prompt",
            cancellationToken: cancellationToken);
    }
}