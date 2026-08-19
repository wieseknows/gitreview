namespace GitReview.Core.Services;

public record ReviewParseResult(string CleanReview, string PatchContent);

public interface IReviewResponseParser
{
    ReviewParseResult Parse(string rawLlmResponse);
}
