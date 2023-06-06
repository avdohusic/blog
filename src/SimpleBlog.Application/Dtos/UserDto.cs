namespace SimpleBlog.Application.Dtos;

public sealed record UserDto
(
    int Id,
    string UserName,
    string Email
);