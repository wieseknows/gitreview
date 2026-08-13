namespace GitReview.Services;

internal interface ILlmReviewService
{
    Task<string> GetReviewAsync(string prompt, CancellationToken cancellationToken = default);
}