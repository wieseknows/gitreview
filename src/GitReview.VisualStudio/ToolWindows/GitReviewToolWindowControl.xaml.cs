using GitReview.VisualStudio.Options;
using GitReview.VisualStudio.Services;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GitReview.VisualStudio.ToolWindows
{
    public partial class GitReviewToolWindowControl : UserControl
    {
        private readonly GitReviewCliRunner _runner = new();
        private CancellationTokenSource? _cts;

        private static readonly Dictionary<string, string[]> ModelsByProvider = new()
        {
            ["openrouter"] =
            [
                "poolside/laguna-s-2.1:free",
                "nvidia/nemotron-3-super:free",
                "cohere/north-mini-code:free",
                "deepseek/deepseek-r1:free"
            ],
            ["gemini"] =
            [
                "gemini-2.0-flash",
                "gemini-1.5-flash",
                "gemini-1.5-pro"
            ],
            ["deepseek"] =
            [
                "deepseek-chat",
                "deepseek-reasoner"
            ]
        };

        public GitReviewToolWindowControl()
        {
            InitializeComponent();
            UpdateModelsForSelectedProvider();
            Log("Ready.");
        }

        private void ModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AiConfigurationPanel == null)
            {
                return;
            }
            AiConfigurationPanel.Visibility = ModeComboBox.SelectedIndex == 0
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void ProviderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateModelsForSelectedProvider();
        }

        private void UpdateModelsForSelectedProvider()
        {
            if (ModelComboBox == null || ProviderComboBox == null)
            {
                return;
            }

            var provider = GetSelectedProviderId();
            if (ModelsByProvider.TryGetValue(provider, out var models))
            {
                ModelComboBox.ItemsSource = models;
                ModelComboBox.SelectedIndex = 0;
            }
        }

        private string GetSelectedProviderId() => ProviderComboBox.SelectedIndex switch
        {
            1 => "gemini",
            2 => "deepseek",
            _ => "openrouter"
        };

        private void OpenSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            GitReviewPackage.Instance?.ShowOptionPage(typeof(GitReviewOptionPage));
        }

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            _ = GitReviewPackage.Instance.JoinableTaskFactory.RunAsync(ReviewAsync);
        }

        private async Task ReviewAsync()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var ct = _cts.Token;

            ReviewButton.IsEnabled = false;
            ReviewButton.Content = "Processing...";
            ClearLog();
            Log("Starting GitReview execution...");

            try
            {
                var solutionDir = GitReviewCliRunner.GetSolutionDirectory();
                if (string.IsNullOrEmpty(solutionDir))
                {
                    Log("[ERROR] Please open a solution first.");
                    return;
                }

                var repoDir = GitReviewCliRunner.FindGitRoot(solutionDir!);
                if (repoDir == null)
                {
                    Log("[ERROR] Solution is not inside a Git repository.");
                    return;
                }

                var args = BuildCliArgs();
                Log($"> git-review {args}");

                int exitCode = await _runner.RunAsync(repoDir, args, LogOnUIThread, ct);

                if (exitCode == 0)
                {
                    Log("\n✅ Execution completed successfully!");
                }
                else
                {
                    Log($"\n❌ Process exited with code {exitCode}");
                }
            }
            catch (OperationCanceledException)
            {
                Log("\n⚠️ Operation cancelled by user.");
            }
            catch (Exception ex)
            {
                Log($"\n❌ [EXCEPTION] {ex.Message}");
            }
            finally
            {
                ReviewButton.IsEnabled = true;
                ReviewButton.Content = "Run GitReview";
            }
        }

        private string BuildCliArgs()
        {
            return ModeComboBox.SelectedIndex switch
            {
                1 => "--prompt-only",
                2 => "raw",
                _ => $"--ai -p {GetSelectedProviderId()} -m \"{ModelComboBox.Text}\""
            };
        }

        private void LogOnUIThread(string text)
        {
            _ = Dispatcher.InvokeAsync(() => Log(text));
        }

        private void Log(string message)
        {
            LogTextBox.AppendText($"{message}\n");
            LogTextBox.ScrollToEnd();
        }

        private void ClearLog()
        {
            LogTextBox.Clear();
        }
    }
}