using GitReview.Helpers;
using GitReview.Models;
using TextCopy;

namespace GitReview.Strategies;

public class RawDiffOutputStrategy : IOutputStrategy
{
    public OutputMode Mode => OutputMode.RawDiffOnly;

    private const string FileName = "git_changes.diff";

    public async Task ProcessAsync(GitDiffResult diff)
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), FileName);

        try
        {
            await File.WriteAllTextAsync(fullPath, diff.CombinedDiff);
            await ClipboardService.SetTextAsync(diff.CombinedDiff);

            Console.WriteLine("✅ Raw diff copied to clipboard");
            Console.WriteLine($"✅ Raw git changes saved to: {fullPath}");

            FileExplorerHelper.OpenAndSelectFile(fullPath);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            Console.WriteLine($"❌ Failed to write diff file: {ex.Message}");
            await ClipboardService.SetTextAsync(diff.CombinedDiff);
            Console.WriteLine("✅ Raw diff copied to clipboard (file write failed)");
        }
    }
}