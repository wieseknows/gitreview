namespace GitReview.Core.Services;

public class ReviewResponseParser : IReviewResponseParser
{
    private const string PatchStartTag = "<patch>";
    private const string PatchEndTag = "</patch>";

    public ReviewParseResult Parse(string rawLlmResponse)
    {
        if (string.IsNullOrWhiteSpace(rawLlmResponse))
        {
            return new ReviewParseResult(string.Empty, string.Empty);
        }

        var start = rawLlmResponse.IndexOf(
            PatchStartTag,
            StringComparison.OrdinalIgnoreCase);

        if (start < 0)
        {
            return new ReviewParseResult(rawLlmResponse.Trim(), string.Empty);
        }

        var patchStart = start + PatchStartTag.Length;

        var end = rawLlmResponse.IndexOf(
            PatchEndTag,
            patchStart,
            StringComparison.OrdinalIgnoreCase);

        if (end < 0)
        {
            return new ReviewParseResult(rawLlmResponse.Trim(), string.Empty);
        }

        var patch = rawLlmResponse[patchStart..end].Trim();
        var review = (
            rawLlmResponse[..start] +
            rawLlmResponse[(end + PatchEndTag.Length)..]
        ).Trim();

        return new ReviewParseResult(review, patch);
    }
}