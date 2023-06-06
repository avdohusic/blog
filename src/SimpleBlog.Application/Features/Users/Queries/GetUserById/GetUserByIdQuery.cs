namespace SimpleBlog.Application.Features.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(int UserId) : IQuery<UserDto>;