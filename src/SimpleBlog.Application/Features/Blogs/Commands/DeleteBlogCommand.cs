using SimpleBlog.Application.Common.Exceptions;
using SimpleBlog.Domain.Repositories;

namespace SimpleBlog.Application.Features.Blogs.Commands;

public sealed record DeleteBlogCommand(Guid BlogId) : ICommand;

internal class DeleteBlogCommandHandler : ICommandHandler<DeleteBlogCommand>
{
    private readonly IBlogRepository _blogRepository;

    public DeleteBlogCommandHandler(IBlogRepository blogRepository)
    {
        this._blogRepository = blogRepository;
    }

    public async Task Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
    {
        var blog = _blogRepository.GetBlogById(request.BlogId);
        if (blog is null)
        {
            throw new NotFoundException(nameof(Blog), request.BlogId);
        }

        await _blogRepository.DeleteBlog(request.BlogId);
    }
}