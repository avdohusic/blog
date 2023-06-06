using AutoMapper;

namespace SimpleBlog.Application.Common.Mappings;

public sealed class BlogProfile : Profile
{
    public BlogProfile()
    {
        CreateMap<Blog, BlogDto>();
    }
}
