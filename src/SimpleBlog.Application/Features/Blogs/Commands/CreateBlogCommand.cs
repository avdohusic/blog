using AutoMapper;
using FluentValidation;
using SimpleBlog.Domain.Repositories;
using SimpleBlog.Domain.Utils;

namespace SimpleBlog.Application.Features.Blogs.Commands;

public sealed record CreateBlogCommand(string Title, string Content, string Author) : ICommand<BlogDto>;

public sealed class CreateBlogCommandValidator : AbstractValidator<CreateBlogCommand>
{
    public CreateBlogCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Content)
            .NotEmpty();

        RuleFor(x => x.Author)
            .NotEmpty()
            .MaximumLength(50);
    }
}

public class CreateBlogCommandHandler : ICommandHandler<CreateBlogCommand, BlogDto>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;

    public CreateBlogCommandHandler(IBlogRepository blogRepository, IMapper mapper)
    {
        _blogRepository = blogRepository;
        _mapper = mapper;
    }

    public async Task<BlogDto> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {
        var blog = Blog.New(request.Title, request.Content)
            .WithAuthor(request.Author)
            .WithPublicationDate(LocalClock.GetTime());

        await _blogRepository.AddBlogAsync(blog);

        var mappedBlog = _mapper.Map<BlogDto>(blog);
        return mappedBlog;
    }
}