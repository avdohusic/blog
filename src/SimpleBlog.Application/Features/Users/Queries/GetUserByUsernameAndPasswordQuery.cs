using Microsoft.AspNetCore.Identity;

namespace SimpleBlog.Application.Features.Users.Queries;

public sealed record GetUserByUsernameAndPasswordQuery(string Username, string Password) : IQuery<UserIdentity>;

public sealed class GetUserByUsernameAndPasswordQueryHandler : IQueryHandler<GetUserByUsernameAndPasswordQuery, UserIdentity>
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly IPasswordHasher<UserIdentity> _passwordHasher;

    public GetUserByUsernameAndPasswordQueryHandler(UserManager<UserIdentity> userManager, IPasswordHasher<UserIdentity> passwordHasher)
    {
        _userManager = userManager;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserIdentity> Handle(GetUserByUsernameAndPasswordQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user != null)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Success)
            {
                return user;
            }
        }

        return await Task.FromResult<UserIdentity>(null);
    }
}