using GitReview.Core.Git;
using GitReview.Core.Models;
using GitReview.Core.Prompt;
using GitReview.Core.Services;
using GitReview.Shared.Enums;

namespace GitReview.Core.Strategies;

public sealed class AiReviewOutputStrategy : IOutputStrategy
{
    private readonly IGitService _gitService;
    private readonly ILlmReviewService _llmService;
    private readonly IPromptBuilder _promptBuilder;
    private readonly IReviewResponseParser _autoFixService;

    public AiReviewOutputStrategy(
        IGitService gitService,
        ILlmReviewService llmService,
        IPromptBuilder promptBuilder,
        IReviewResponseParser autoFixService)
    {
        _gitService = gitService;
        _llmService = llmService;
        _promptBuilder = promptBuilder;
        _autoFixService = autoFixService;
    }

    public ReviewExecutionMode Mode => ReviewExecutionMode.AiReview;

    public async Task ProcessAsync(GitDiffResult diff, CancellationToken cancellationToken = default)
    {
        var prompt = _promptBuilder.Build(
            diff,
            _gitService.GetRepositoryRoot(),
            _gitService.GetCurrentBranch());

        Console.WriteLine("🤖 Requesting code review from AI...");
        var rawResponse = await _llmService.GetReviewAsync(prompt, cancellationToken);

        // Separate clean text from patch block
        var (cleanReview, patchContent) = _autoFixService.Parse(rawResponse);

        // Display formatted markdown review to stdout
        Console.WriteLine();
        Console.WriteLine("========= AI CODE REVIEW =========");
        Console.WriteLine(cleanReview);
        Console.WriteLine("==================================");
        Console.WriteLine();

        // Prompt user if a valid patch was extracted
        if (string.IsNullOrWhiteSpace(patchContent))
        {
            return;
        }

        Console.WriteLine("💡 AI suggested automatic code fixes.");
        Console.Write("Do you want to apply suggested changes to your working directory? [y/N]: ");

        var input = Console.ReadLine()?.Trim().ToLowerInvariant();
        if (input == "y" || input == "yes")
        {
            Console.WriteLine("🛠 Applying patch via git apply...");

            try
            {
                _gitService.ApplyPatch(patchContent);
                Console.WriteLine("✅ Suggested changes successfully applied! Use 'git diff' to review changes.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Failed to apply patch cleanly.");
                Console.WriteLine($"Git Error:\n{ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Skipped applying patch.");
        }
    }
}