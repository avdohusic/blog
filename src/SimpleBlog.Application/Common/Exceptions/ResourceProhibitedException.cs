namespace SimpleBlog.Application.Common.Exceptions;

public sealed class ResourceProhibitedException : Exception
{
    public ResourceProhibitedException()
        : base()
    {

    }

    public ResourceProhibitedException(string message)
    : base(message)
    {
    }
}
