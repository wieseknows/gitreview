using GitReview.Core.Models;
using System.Diagnostics;
using System.Text;

namespace GitReview.Core.Git;

public sealed class GitService : IGitService
{
    private const int TimeoutMs = 30_000;

    public GitDiffResult GetDiff()
    {
        if (!IsGitRepository())
        {
            throw new InvalidOperationException("Current directory is not a Git repository.");
        }

        return new GitDiffResult
        {
            StagedDiff = ExecuteGit("diff", "--cached"),
            WorkingTreeDiff = ExecuteGit("diff")
        };
    }

    public string GetRepositoryRoot()
    {
        return ExecuteGit("rev-parse", "--show-toplevel").Trim();
    }

    public string GetCurrentBranch()
    {
        return ExecuteGit("branch", "--show-current").Trim();
    }

    public bool IsGitRepository()
    {
        try
        {
            var result = ExecuteGit("rev-parse", "--is-inside-work-tree");
            return result
                .Trim()
                .Equals(true.ToString(), StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    public void ApplyPatch(string patchContent)
    {
        if (string.IsNullOrWhiteSpace(patchContent))
        {
            throw new ArgumentException("Patch content is empty.", nameof(patchContent));
        }

        var sanitized = patchContent
            .Replace('\u00A0', ' ')
            .Replace("\r\n", "\n");

        if (!sanitized.EndsWith('\n'))
        {
            sanitized += "\n";
        }

        if (!sanitized.TrimStart().StartsWith("diff --git ", StringComparison.Ordinal))
        {
            throw new ArgumentException("Invalid Git unified diff.", nameof(patchContent));
        }

        var tempPatchPath = Path.Combine(
            Path.GetTempPath(),
            $"gitreview_fix_{Guid.NewGuid():N}.patch");

        try
        {
            File.WriteAllText(tempPatchPath, sanitized);

            // Last argument is the path to the patch file (git apply <file>)
            ExecuteGit(
                "apply",
                "--ignore-space-change",
                "--ignore-whitespace",
                "--inaccurate-eof",
                "--whitespace=nowarn",
                "--unidiff-zero",
                "--recount",
                tempPatchPath);
        }
        finally
        {
            try
            {
                if (File.Exists(tempPatchPath))
                {
                    File.Delete(tempPatchPath);
                }
            }
            catch { }
        }
    }

    private string ExecuteGit(params string[] args)
    {
        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = "git",
            WorkingDirectory = Directory.GetCurrentDirectory(),
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach (var arg in args)
        {
            process.StartInfo.ArgumentList.Add(arg);
        }

        var outputBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                outputBuilder.AppendLine(e.Data);
            }
        };

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
            {
                errorBuilder.AppendLine(e.Data);
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        if (!process.WaitForExit(TimeoutMs))
        {
            try
            {
                process.Kill(entireProcessTree: true);
            }
            catch { }

            throw new TimeoutException($"Git command timed out after {TimeoutMs}ms: git {string.Join(" ", args)}");
        }

        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"Git command failed (exit code {process.ExitCode}): git {string.Join(" ", args)}\n{errorBuilder}");
        }

        return outputBuilder.ToString();
    }
}