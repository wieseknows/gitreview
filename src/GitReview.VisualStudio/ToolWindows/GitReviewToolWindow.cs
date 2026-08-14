using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace GitReview.VisualStudio.ToolWindows
{
    [Guid("a7b9c2e1-5d34-4f86-91ab-27c8e6f40b52")]
    public class GitReviewToolWindow : ToolWindowPane
    {
        public GitReviewToolWindow()
            : base(null)
        {
            Caption = "GitReview";
            Content = new GitReviewToolWindowControl();
        }
    }
}