using GitReview.Core.Exceptions;
using GitReview.Core.Git;
using GitReview.Shared.Enums;

namespace GitReview.Core.Models;

public sealed class ReviewCommand
{
    private readonly IGitService _gitService;
    private readonly IReadOnlyDictionary<ReviewExecutionMode, IOutputStrategy> _strategies;

    public ReviewCommand(IGitService gitService, IEnumerable<IOutputStrategy> strategies)
    {
        _gitService = gitService;
        try
        {
            _strategies = strategies.ToDictionary(s => s.Mode);
        }
        catch (ArgumentException ex)
        {
            throw new InvalidOperationException(
                $"Duplicate {nameof(IOutputStrategy)}.{nameof(IOutputStrategy.Mode)} registration. Each OutputMode must map to exactly one strategy.",
                ex);
        }
    }

    public async Task ExecuteAsync(ReviewExecutionMode mode, CancellationToken cancellationToken)
    {
        var diff = _gitService.GetDiff();

        Console.WriteLine($"Changed files: {diff.ChangedFiles}");
        Console.WriteLine($"Changed lines: {diff.ChangedLines}");

        if (!diff.HasChanges)
        {
            Console.WriteLine("No changes found.");
            return;
        }

        if (!_strategies.TryGetValue(mode, out var strategy))
        {
            Console.WriteLine($"❌ No strategy found for mode: {mode}");
            return;
        }

        try
        {
            await strategy.ProcessAsync(diff, cancellationToken);
        }
        catch (LlmTimeoutException ex)
        {
            Console.WriteLine();
            Console.WriteLine($"⏱️  Timeout Error: {ex.Message}");
            Console.WriteLine("    The AI provider took too long to respond. Please try again later or select a faster model/provider.");
        }
        catch (LlmApiException ex)
        {
            Console.WriteLine();
            Console.WriteLine($"❌ AI Provider Error: {ex.Message}");
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine();
            Console.WriteLine("⚠️  Operation was cancelled by user.");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine($"💥 An unexpected error occurred: {ex.Message}");
        }
    }
}