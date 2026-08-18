using GitReview.Shared.Enums;
using GitReview.Shared.Providers;

namespace GitReview.Core.Models;

public sealed record ReviewOptions(
    ReviewExecutionMode Mode = ReviewExecutionMode.PromptWithClipboard,
    AiProvider Provider = ProviderRegistry.DefaultProvider);