using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace SimpleBlog.Application.Features.Users.Queries;

public sealed record GetUserByIdQuery(int UserId) : IQuery<UserDto>;

public sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(UserManager<UserIdentity> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }
}