using SimpleBlog.Application.Common.Exceptions;
using SimpleBlog.Domain.Repositories;

namespace SimpleBlog.Application.Features.Blogs.Commands;

public sealed record DeleteBlogCommand(Guid BlogId) : ICommand;

public class DeleteBlogCommandHandler : ICommandHandler<DeleteBlogCommand>
{
    private readonly IBlogRepository _blogRepository;

    public DeleteBlogCommandHandler(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
    {
        var blog = _blogRepository.GetBlogByIdAsync(request.BlogId);
        if (blog is null)
        {
            throw new NotFoundException(nameof(Blog), request.BlogId);
        }

        await _blogRepository.DeleteBlogAsync(request.BlogId);
    }
}