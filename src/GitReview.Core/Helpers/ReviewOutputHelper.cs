using TextCopy;

namespace GitReview.Core.Helpers;

public static class ReviewOutputHelper
{
    public static async Task SaveClipboardAndRevealAsync(
        string content,
        string fileName,
        string successLabel,
        CancellationToken cancellationToken = default)
    {
        var dir = Environment.GetEnvironmentVariable("GIT_REVIEW_OUT") ?? Directory.GetCurrentDirectory();
        Directory.CreateDirectory(dir);

        var fullPath = Path.Combine(dir, fileName);

        var written = false;
        try
        {
            await File.WriteAllTextAsync(fullPath, content, cancellationToken);
            Console.WriteLine($"✅ {successLabel} saved to: {fullPath}");

            written = true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            Console.WriteLine($"⚠️  File write failed: {ex.Message}");
        }

        try
        {
            await ClipboardService.SetTextAsync(content, cancellationToken);
            Console.WriteLine("✅ Copied to clipboard");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️  Clipboard failed: {ex.Message}");
        }

        if (written && File.Exists(fullPath))
        {
            FileExplorerHelper.OpenAndSelectFile(fullPath);
        }
    }
}