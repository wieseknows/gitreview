using System.ComponentModel;

namespace GitReview.Shared.Enums
{
    public enum AiProvider
    {
        [Description("OpenRouter")]
        OpenRouter = 0,

        [Description("Google Gemini")]
        Gemini = 1,

        [Description("DeepSeek")]
        DeepSeek = 2
    }
}
