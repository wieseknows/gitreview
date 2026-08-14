using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace GitReview.VisualStudio
{
    [PackageRegistration(
        UseManagedResourcesOnly = true,
        AllowsBackgroundLoading = true)]

    [InstalledProductRegistration(
        "GitReview",
        "AI-powered code review for Visual Studio",
        "1.0")]

    [ProvideMenuResource("Menus.ctmenu", 1)]

    [ProvideToolWindow(typeof(ToolWindows.GitReviewToolWindow))]

    [Guid("d3b07384-d11e-49fb-bc3f-fb112b329437")]
    public sealed class GitReviewPackage : AsyncPackage
    {
        public static GitReviewPackage Instance { get; private set; } = null!;

        protected override async Task InitializeAsync(
            CancellationToken cancellationToken,
            IProgress<ServiceProgressData> progress)
        {
            Instance = this;

            await GitReviewCommand.InitializeAsync(this);
        }

        internal async Task ShowGitReviewAsync()
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync();

            await ShowToolWindowAsync(
                typeof(ToolWindows.GitReviewToolWindow),
                0,
                true,
                CancellationToken.None);
        }
    }
}