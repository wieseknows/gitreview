using GitReview.Shared.Constants;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel;

namespace GitReview.VisualStudio.Options
{
    // Generates a settings tab in Tools -> Options -> GitReview -> General
    public class GitReviewOptionPage : DialogPage
    {
        [Category("API Keys")]
        [DisplayName("OpenRouter API Key")]
        [Description($"Set {EnvVariables.OpenRouterApiKey} for OpenRouter requests.")]
        public string OpenRouterApiKey { get; set; } = string.Empty;

        [Category("API Keys")]
        [DisplayName("Gemini API Key")]
        [Description($"Set {EnvVariables.GeminiApiKey} for Google Gemini requests.")]
        public string GeminiApiKey { get; set; } = string.Empty;

        [Category("API Keys")]
        [DisplayName("DeepSeek API Key")]
        [Description($"Set {EnvVariables.DeepSeekApiKey} for DeepSeek requests.")]
        public string DeepSeekApiKey { get; set; } = string.Empty;
    }
}