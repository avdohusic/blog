using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace SimpleBlog.Application.Features.Users.Commands;

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

public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserDto>
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(UserManager<UserIdentity> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new UserIdentity
        {
            UserName = request.Username,
            Email = request.Email
        };

        var createdUser = await _userManager.CreateAsync(user, request.Password);
        if (!createdUser.Succeeded)
        {
            throw new ValidationException(createdUser.Errors.Select(x => new ValidationFailure()
            {
                PropertyName = "",
                ErrorCode = x.Code,
                ErrorMessage = x.Description
            }));
        }
        var userDto = _mapper.Map<UserDto>(user);
        return userDto;
    }
}