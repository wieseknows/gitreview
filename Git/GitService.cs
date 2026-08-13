using GitReview.Models;
using System.Diagnostics;
using System.Text;

namespace GitReview.Git;

internal sealed class GitService : IGitService
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
            StagedDiff = ExecuteGit("diff --cached"),
            WorkingTreeDiff = ExecuteGit("diff")
        };
    }

    public string GetRepositoryRoot()
    {
        return ExecuteGit("rev-parse --show-toplevel").Trim();
    }

    public string GetCurrentBranch()
    {
        return ExecuteGit("branch --show-current").Trim();
    }

    public bool IsGitRepository()
    {
        try
        {
            var result = ExecuteGit("rev-parse --is-inside-work-tree");
            return result
                .Trim()
                .Equals("true", StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    private string ExecuteGit(string arguments)
    {
        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = arguments,
            WorkingDirectory = Directory.GetCurrentDirectory(),
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

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
            catch
            {
                // ignore
            }

            throw new TimeoutException($"Git command timed out after {TimeoutMs}ms: git {arguments}");
        }

        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"Git command failed (exit code {process.ExitCode}): git {arguments}\n{errorBuilder}");
        }

        return outputBuilder.ToString();
    }
}