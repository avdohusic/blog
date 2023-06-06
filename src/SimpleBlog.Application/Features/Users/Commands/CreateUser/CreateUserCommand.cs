using FluentValidation;

namespace SimpleBlog.Application.Features.Users.Commands.CreateUser;

public sealed record CreateUserCommand(string Username, string Password, string Email) : ICommand<UserDto>;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(50)
            .EmailAddress();
    }
}