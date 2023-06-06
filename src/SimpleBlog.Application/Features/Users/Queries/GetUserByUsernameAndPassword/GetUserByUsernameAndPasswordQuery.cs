using Microsoft.AspNetCore.Identity;

namespace SimpleBlog.Application.Features.Users.Queries.GetUserByUsernameAndPassword;

public sealed record GetUserByUsernameAndPasswordQuery(string Username, string Password) : IQuery<UserIdentity>;
