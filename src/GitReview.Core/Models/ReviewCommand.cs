using GitReview.Core.Git;

namespace GitReview.Core.Models;

public sealed class ReviewCommand
{
    private readonly IGitService _gitService;
    private readonly IReadOnlyDictionary<OutputMode, IOutputStrategy> _strategies;

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

    public async Task ExecuteAsync(ReviewOptions options, CancellationToken cancellationToken)
    {
        if (!_gitService.IsGitRepository())
        {
            Console.WriteLine("❌ Not a git repository");
            return;
        }

        var diff = _gitService.GetDiff();

        Console.WriteLine($"Changed files: {diff.ChangedFiles}");
        Console.WriteLine($"Changed lines: {diff.ChangedLines}");

        if (!diff.HasChanges)
        {
            Console.WriteLine("No changes found.");
            return;
        }

        if (!_strategies.TryGetValue(options.Mode, out var strategy))
        {
            Console.WriteLine($"❌ No strategy found for mode: {options.Mode}");
            return;
        }

        await strategy.ProcessAsync(diff, cancellationToken);
    }
}