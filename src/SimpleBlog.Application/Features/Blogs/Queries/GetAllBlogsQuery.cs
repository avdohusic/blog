using AutoMapper;
using SimpleBlog.Domain.Repositories;

namespace SimpleBlog.Application.Features.Blogs.Queries;

public sealed record GetAllBlogsQuery : IQuery<IEnumerable<BlogDto>>;

public class GetAllBlogsQueryHandler : IQueryHandler<GetAllBlogsQuery, IEnumerable<BlogDto>>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;

    public GetAllBlogsQueryHandler(IBlogRepository blogRepository, IMapper mapper)
    {
        this._blogRepository = blogRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BlogDto>> Handle(GetAllBlogsQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<IEnumerable<BlogDto>>(await _blogRepository.GetAllBlogs());
    }
}