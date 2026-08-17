using System.ComponentModel;

namespace GitReview.Shared.Enums
{
    public enum ReviewExecutionMode
    {
        [Description("🤖 AI Review (Full Analysis)")]
        AiReview = 0,

        [Description("📝 Generate Prompt Only")]
        PromptWithClipboard = 1,

        [Description("📄 Raw Git Diff")]
        RawDiffOnly = 2
    }
}
