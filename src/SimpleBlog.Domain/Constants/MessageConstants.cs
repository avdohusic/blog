namespace SimpleBlog.Domain.Constants;

public static class MessageConstants
{
    public const string ValidationTitle = "Validation Failure";
    public const string ValidationMessage = "One or more validation errors occurred.";

    public const string ServerErrorTitle = "Server Error";
    public const string ServerErrorMessage = "An internal server error has occurred.";

    public const string BadRequestTitle = "Bad Request";

    public const string NotFoundTitle = "Not Found";
    public const string NotFoundMessage = "We're sorry, but the requested resource could not be found.";

    public const string UnauthorizedTitle = "User not allowed or not logged in.";
    public const string UnauthorizedMessage = "We're sorry, but the request cannot be realized due permissions rule.";
}