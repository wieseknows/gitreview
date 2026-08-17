using GitReview.Shared.Constants;
using GitReview.VisualStudio.Options;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GitReview.VisualStudio.Services
{
    public class GitReviewCliRunner
    {
        public async Task<int> RunAsync(string repoDir, string args, Action<string> logCallback, CancellationToken ct)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "git-review",
                Arguments = args,
                WorkingDirectory = repoDir,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            ApplyOptionsEnvironmentVariables(psi);

            using var process = new Process { StartInfo = psi, EnableRaisingEvents = true };

            process.OutputDataReceived += (_, e) =>
            {
                if (e.Data != null)
                {
                    logCallback(e.Data);
                }
            };
            process.ErrorDataReceived += (_, e) =>
            {
                if (e.Data != null)
                {
                    logCallback($"[ERROR] {e.Data}");
                }
            };

            if (!process.Start())
            {
                throw new InvalidOperationException("Failed to start git-review process.");
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await Task.Run(() =>
            {
                while (!process.WaitForExit(500))
                {
                    if (ct.IsCancellationRequested)
                    {
                        try { process.Kill(); } catch { }
                        ct.ThrowIfCancellationRequested();
                    }
                }
            }, ct);

            return process.ExitCode;
        }

        private static void ApplyOptionsEnvironmentVariables(ProcessStartInfo psi)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var options = (GitReviewOptionPage)GitReviewPackage.Instance.GetDialogPage(typeof(GitReviewOptionPage));

            if (options == null)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(options.OpenRouterApiKey))
            {
                psi.EnvironmentVariables[EnvVariables.OpenRouterApiKey] = options.OpenRouterApiKey;
            }
            if (!string.IsNullOrWhiteSpace(options.GeminiApiKey))
            {
                psi.EnvironmentVariables[EnvVariables.GeminiApiKey] = options.GeminiApiKey;
            }

            if (!string.IsNullOrWhiteSpace(options.DeepSeekApiKey))
            {
                psi.EnvironmentVariables[EnvVariables.DeepSeekApiKey] = options.DeepSeekApiKey;
            }
        }

        public static string? GetSolutionDirectory()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var solution = Package.GetGlobalService(typeof(SVsSolution)) as IVsSolution;
            if (solution == null)
            {
                return null;
            }

            var hr = solution.GetSolutionInfo(out var dir, out var file, out _);
            if (ErrorHandler.Failed(hr))
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(dir))
            {
                return dir.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar, ' ', '\0');
            }
            if (!string.IsNullOrWhiteSpace(file))
            {
                return Path.GetDirectoryName(file);
            }

            return null;
        }

        public static string? FindGitRoot(string startDir)
        {
            var dir = new DirectoryInfo(startDir);
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, ".git")))
                {
                    return dir.FullName;
                }
                dir = dir.Parent;
            }
            return null;
        }
    }
}