using AutoMapper;
using FluentValidation;
using SimpleBlog.Application.Extensions;
using SimpleBlog.Domain.Constants;
using SimpleBlog.Domain.Repositories;

namespace SimpleBlog.Application.Features.Blogs.Commands;

public sealed record ImportBlogCommand(Stream Stream) : ICommand;

public sealed class ImportBlogCommandValidator : AbstractValidator<ImportBlogCommand>
{
    public ImportBlogCommandValidator()
    {
        RuleFor(x => x.Stream)
            .NotNull();
    }
}

internal class ImportBlogCommandHandler : ICommandHandler<ImportBlogCommand>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;

    public ImportBlogCommandHandler(IBlogRepository blogRepository, IMapper mapper)
    {
        _blogRepository = blogRepository;
        _mapper = mapper;
    }

    public async Task Handle(ImportBlogCommand request, CancellationToken cancellationToken)
    {
        var blogItems = request.Stream.ReadDataFromExcelFile<BlogExcelDto>(GlobalConstants.BlogTableName);
        if (blogItems.Any())
        {
            foreach (var blogItem in blogItems)
            {
                if (blogItem.BlogId.HasValue) //Perhaps that Blog exists in a database, in the real scenario, we should check if really exist.
                {
                    await _blogRepository.UpdateBlog(blogItem.BlogId.Value, _mapper.Map<Blog>(blogItem));
                }
                else
                {
                    await _blogRepository.AddBlog(_mapper.Map<Blog>(blogItem));
                }
            }
        }
    }
}