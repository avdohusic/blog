using AutoMapper;
using FluentValidation;
using SimpleBlog.Domain.Repositories;

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

internal class CreateBlogCommandHandler : ICommandHandler<CreateBlogCommand, BlogDto>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;

    public CreateBlogCommandHandler(IBlogRepository blogRepository, IMapper mapper)
    {
        this._blogRepository = blogRepository;
        _mapper = mapper;
    }

    public async Task<BlogDto> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {
        var blog = new Blog
        {
            Title = request.Title,
            Content = request.Content,
            Author = request.Author,
            PublicationDate = DateTime.Now
        };

        await _blogRepository.AddBlog(blog);

        return _mapper.Map<BlogDto>(blog);
    }
}