using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GitReview.VisualStudio.ToolWindows
{
    public partial class GitReviewToolWindowControl : UserControl
    {
        public GitReviewToolWindowControl()
        {
            InitializeComponent();
        }

        private async void ReviewButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            ReviewButton.IsEnabled = false;
            ReviewButton.Content = "Reviewing...";

            ResultsPanel.Children.Clear();

            var loadingText = new TextBlock
            {
                Text = "Analyzing changes...",
                Opacity = 0.6,
                Margin = new Thickness(0, 0, 0, 12)
            };

            ResultsPanel.Children.Add(loadingText);

            try
            {
                await Task.Delay(1000);

                ResultsPanel.Children.Clear();

                AddFinding(
                    "High",
                    "Possible null reference",
                    "src/GitReview.Core/ReviewEngine.cs",
                    42,
                    "The value may be null when this code path is executed.",
                    "Consider checking the value before accessing its members.");

                AddFinding(
                    "Medium",
                    "Method is doing too much",
                    "src/GitReview.Core/ReviewEngine.cs",
                    87,
                    "This method appears to handle several independent responsibilities.",
                    "Consider extracting the individual operations into separate methods.");

                AddFinding(
                    "Low",
                    "Naming could be clearer",
                    "src/GitReview.Core/ReviewEngine.cs",
                    103,
                    "The variable name does not clearly communicate its purpose.",
                    "Use a more descriptive name.");
            }
            finally
            {
                ReviewButton.IsEnabled = true;
                ReviewButton.Content = "Review Changes";
            }
        }

        private void AddFinding(
            string severity,
            string title,
            string file,
            int line,
            string description,
            string suggestion)
        {
            var panel = new Border
            {
                BorderThickness = new Thickness(1),
                Padding = new Thickness(12),
                Margin = new Thickness(0, 0, 0, 10)
            };

            var stack = new StackPanel();

            stack.Children.Add(new TextBlock
            {
                Text = $"{severity}  •  {title}",
                FontWeight = FontWeights.SemiBold,
                FontSize = 14
            });

            stack.Children.Add(new TextBlock
            {
                Text = $"{file}:{line}",
                Opacity = 0.6,
                Margin = new Thickness(0, 4, 0, 8)
            });

            stack.Children.Add(new TextBlock
            {
                Text = description,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 8)
            });

            stack.Children.Add(new TextBlock
            {
                Text = $"Suggestion: {suggestion}",
                TextWrapping = TextWrapping.Wrap,
                Opacity = 0.8
            });

            panel.Child = stack;

            ResultsPanel.Children.Add(panel);
        }
    }
}