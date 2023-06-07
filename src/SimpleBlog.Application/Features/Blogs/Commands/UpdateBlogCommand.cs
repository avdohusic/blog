using AutoMapper;
using FluentValidation;
using SimpleBlog.Application.Common.Exceptions;
using SimpleBlog.Domain.Repositories;

namespace SimpleBlog.Application.Features.Blogs.Commands;

public sealed class UpdateBlogCommand : ICommand<BlogDto>
{
    public Guid BlogId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
}

public sealed class UpdateBlogCommandValidator : AbstractValidator<CreateBlogCommand>
{
    public UpdateBlogCommandValidator()
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

public class UpdateBlogCommandHandler : ICommandHandler<UpdateBlogCommand, BlogDto>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;

    public UpdateBlogCommandHandler(IBlogRepository blogRepository, IMapper mapper)
    {
        this._blogRepository = blogRepository;
        this._mapper = mapper;
    }

    public async Task<BlogDto> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        var existingBlog = await _blogRepository.GetBlogById(request.BlogId);
        if (existingBlog == null)
        {
            throw new NotFoundException(nameof(Blog), request.BlogId);
        }

        existingBlog.Title = request.Title;
        existingBlog.Content = request.Content;
        existingBlog.Author = request.Author;

        await _blogRepository.UpdateBlog(request.BlogId, existingBlog);
        return _mapper.Map<BlogDto>(existingBlog);
    }
}