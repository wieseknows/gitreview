using GitReview.Git;
using GitReview.Helpers;
using GitReview.Models;
using GitReview.Prompt;
using TextCopy;

namespace GitReview.Strategies;

public class PromptOutputStrategy : IOutputStrategy
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

    public async Task ProcessAsync(GitDiffResult diff)
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), FileName);

        var prompt = _promptBuilder.Build(
            diff,
            _gitService.GetRepositoryRoot(),
            _gitService.GetCurrentBranch());

        try
        {
            await File.WriteAllTextAsync(fullPath, prompt);
            await ClipboardService.SetTextAsync(prompt);

            Console.WriteLine("✅ Prompt copied to clipboard");
            Console.WriteLine($"✅ Prompt saved to: {fullPath}");

            FileExplorerHelper.OpenAndSelectFile(fullPath);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            Console.WriteLine($"❌ Failed to write prompt file: {ex.Message}");
            await ClipboardService.SetTextAsync(prompt);
            Console.WriteLine("✅ Prompt copied to clipboard (file write failed)");
        }
    }
}