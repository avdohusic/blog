using Microsoft.AspNetCore.Identity;

namespace SimpleBlog.Application.Features.Users.Queries;

public sealed record GetUserRolesByUsernameQuery(string Username) : IQuery<string>;

public sealed class GetUserRolesByUsernameQueryHandler : IQueryHandler<GetUserRolesByUsernameQuery, string>
{
    private readonly UserManager<UserIdentity> _userManager;

    public GetUserRolesByUsernameQueryHandler(UserManager<UserIdentity> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> Handle(GetUserRolesByUsernameQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles == null)
        {
            return string.Empty;
        }

        return string.Join(",", userRoles);
    }
}