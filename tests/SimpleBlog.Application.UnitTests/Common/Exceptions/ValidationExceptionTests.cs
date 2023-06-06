using FluentValidation.Results;
using ValidationException = SimpleBlog.Application.Common.Exceptions.ValidationException;

namespace Application.UnitTests.Common.Exceptions;

public class ValidationExceptionTests
{
    [Fact]
    public void DefaultConstructorCreatesAnEmptyErrorDictionary()
    {
        var actual = new ValidationException().Errors;

        actual.Keys.Should().BeEquivalentTo(Array.Empty<string>());
    }

    [Fact]
    public void SingleValidationFailureCreatesASingleElementErrorDictionary()
    {
        var failures = new List<ValidationFailure>
        {
            new ValidationFailure("Username", "must be 50 characters or fewer"),
        };

        var actual = new ValidationException(failures).Errors;

        actual.Keys.Should().BeEquivalentTo("Username");
        actual["Username"].Should().BeEquivalentTo("must be 50 characters or fewer");
    }

    [Fact]
    public void MulitpleValidationFailureForMultiplePropertiesCreatesAMultipleElementErrorDictionaryEachWithMultipleValues()
    {
        var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Username", "must be 50 characters or fewer"),
                new ValidationFailure("Username", "must follow naming rules"),
                new ValidationFailure("Password", "must contain at least 8 characters"),
                new ValidationFailure("Password", "must contain a digit"),
                new ValidationFailure("Password", "must contain upper case letter"),
                new ValidationFailure("Password", "must contain lower case letter"),
            };

        var actual = new ValidationException(failures).Errors;

        actual.Keys.Should().BeEquivalentTo("Username", "Password");

        actual["Username"].Should().BeEquivalentTo(
            "must be 50 characters or fewer",
            "must follow naming rules"
        );

        actual["Password"].Should().BeEquivalentTo(
            "must contain lower case letter",
            "must contain upper case letter",
            "must contain at least 8 characters",
            "must contain a digit"
        );
    }
}
