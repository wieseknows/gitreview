using Microsoft.VisualStudio.Shell;
using System.ComponentModel;

namespace GitReview.VisualStudio.Options
{
    // Generates a settings tab in Tools -> Options -> GitReview -> General
    public class GitReviewOptionPage : DialogPage
    {
        [Category("API Keys")]
        [DisplayName("OpenRouter API Key")]
        [Description("Set OPENROUTER_API_KEY for OpenRouter requests.")]
        public string OpenRouterApiKey { get; set; } = string.Empty;

        [Category("API Keys")]
        [DisplayName("Gemini API Key")]
        [Description("Set GEMINI_API_KEY for Google Gemini requests.")]
        public string GeminiApiKey { get; set; } = string.Empty;

        [Category("API Keys")]
        [DisplayName("DeepSeek API Key")]
        [Description("Set DEEPSEEK_API_KEY for DeepSeek requests.")]
        public string DeepSeekApiKey { get; set; } = string.Empty;
    }
}