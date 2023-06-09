using AutoMapper;
using SimpleBlog.Application.Common.Exceptions;
using SimpleBlog.Domain.Repositories;

namespace SimpleBlog.Application.Features.Blogs.Queries;

public sealed record GetBlogByIdQuery(Guid BlogId) : IQuery<BlogDto>;

public class GetBlogByIdQueryHandler : IQueryHandler<GetBlogByIdQuery, BlogDto>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;

    public GetBlogByIdQueryHandler(IBlogRepository blogRepository, IMapper mapper)
    {
        _blogRepository = blogRepository;
        _mapper = mapper;
    }

    public async Task<BlogDto> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
    {
        var blog = await _blogRepository.GetBlogById(request.BlogId);
        if (blog is null)
        {
            throw new NotFoundException(nameof(Blog), request.BlogId);
        }
        return _mapper.Map<BlogDto>(blog);
    }
}