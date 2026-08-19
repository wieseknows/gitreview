namespace GitReview.Core.Exceptions;

public class LlmApiException : Exception
{
    public LlmApiException(string message, Exception? innerException = null)
        : base(message, innerException) { }
}