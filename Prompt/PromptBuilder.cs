using GitReview.Models;

namespace GitReview.Prompt;

public class PromptBuilder : IPromptBuilder
{
    private readonly string _templatePath;

    public PromptBuilder()
    {
        _templatePath = Path.Combine(
            AppContext.BaseDirectory,
            "Templates",
            "prompt.md");
    }

    public string Build(
        GitDiffResult diff,
        string repository,
        string branch)
    {
        if (!File.Exists(_templatePath))
        {
            throw new FileNotFoundException(
                "Prompt template not found",
                _templatePath);
        }

        var template = File.ReadAllText(_templatePath);

        return template
            .Replace("{{REPOSITORY}}", repository)
            .Replace("{{BRANCH}}", branch)
            .Replace("{{FILES}}", diff.ChangedFiles.ToString())
            .Replace("{{LINES}}", diff.ChangedLines.ToString())
            .Replace("{{DIFF}}", diff.CombinedDiff);
    }
}