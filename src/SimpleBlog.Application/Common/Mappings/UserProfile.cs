using AutoMapper;

namespace SimpleBlog.Application.Common.Mappings;

public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserIdentity, UserDto>();
    }
}