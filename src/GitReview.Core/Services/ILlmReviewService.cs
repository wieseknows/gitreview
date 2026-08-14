namespace GitReview.Core.Services;

public interface ILlmReviewService
{
    Task<string> GetReviewAsync(string prompt, CancellationToken cancellationToken = default);
}