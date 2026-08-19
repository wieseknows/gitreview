namespace GitReview.Core.Exceptions;

public class LlmTimeoutException : Exception
{
    public LlmTimeoutException(string message, Exception? innerException = null)
        : base(message, innerException) { }
}
