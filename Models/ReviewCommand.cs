using GitReview.Git;
using GitReview.Strategies;

namespace GitReview.Models;

public class ReviewCommand
{
    private readonly IGitService _gitService;
    private readonly IEnumerable<IOutputStrategy> _strategies;

    public ReviewCommand(IGitService gitService, IEnumerable<IOutputStrategy> strategies)
    {
        _gitService = gitService;
        _strategies = strategies;
    }

    public async Task ExecuteAsync(ReviewOptions options)
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

        var strategy = _strategies.FirstOrDefault(s => s.Mode == options.Mode);
        if (strategy == null)
        {
            Console.WriteLine($"❌ No strategy found for mode: {options.Mode}");
            return;
        }

        await strategy.ProcessAsync(diff);
    }
}