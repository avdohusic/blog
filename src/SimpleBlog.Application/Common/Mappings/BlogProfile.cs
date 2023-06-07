﻿using AutoMapper;

namespace SimpleBlog.Application.Common.Mappings;

public sealed class BlogProfile : Profile
{
    public BlogProfile()
    {
        CreateMap<Blog, BlogDto>()
            .ConstructUsing(blog => new BlogDto(blog.Id, blog.Title, blog.Content, blog.Author, blog.PublicationDate))
            .ReverseMap();
    }
}