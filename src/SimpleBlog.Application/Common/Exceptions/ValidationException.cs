using SimpleBlog.Domain.Constants;
using FluentValidation.Results;

namespace SimpleBlog.Application.Common.Exceptions;

public sealed class ValidationException : Exception
{
    public ValidationException()
        : base(MessageConstants.ValidationMessage)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}
