using GitReview.Git;
using GitReview.Helpers;
using GitReview.Models;
using GitReview.Prompt;
using GitReview.Services;

namespace GitReview.Strategies;

internal sealed class AiReviewOutputStrategy : IOutputStrategy
{
    private readonly IPromptBuilder _promptBuilder;
    private readonly IGitService _gitService;
    private readonly ILlmReviewService _llm;

    public OutputMode Mode => OutputMode.AiReview;

    private const string FileName = "ai_review_result.md";

    public AiReviewOutputStrategy(
        IPromptBuilder promptBuilder,
        IGitService gitService,
        ILlmReviewService llm)
    {
        _promptBuilder = promptBuilder;
        _gitService = gitService;
        _llm = llm;
    }

    public async Task ProcessAsync(GitDiffResult diff, CancellationToken cancellationToken = default)
    {
        var prompt = _promptBuilder.Build(
            diff,
            _gitService.GetRepositoryRoot(),
            _gitService.GetCurrentBranch());

        Console.WriteLine("🤖 Analyzing code changes with AI...");

        try
        {
            var aiReview = await _llm.GetReviewAsync(prompt, cancellationToken);

            await ReviewOutputHelper.SaveClipboardAndRevealAsync(
                content: aiReview,
                fileName: FileName,
                successLabel: "AI review",
                cancellationToken: cancellationToken);

            Console.WriteLine();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"❌ LLM HTTP error: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"⚠️  {ex.Message}");
            Console.WriteLine("🔑 Gemini key: https://aistudio.google.com/app/apikey");
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine("⚠️  Cancelled.");
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("❌ AI request timed out");
        }
    }
}