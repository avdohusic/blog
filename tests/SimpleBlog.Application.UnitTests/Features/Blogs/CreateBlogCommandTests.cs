using AutoMapper;
using MediatR;
using Moq;
using SimpleBlog.Application.Common.Mappings;
using SimpleBlog.Application.Features.Blogs.Commands;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Repositories;

namespace SimpleBlog.Application.UnitTests.Features.Blogs;
public class CreateBlogCommandTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;
    public CreateBlogCommandTests()
    {
        _configuration = new MapperConfiguration(config =>
        {
            config.AddProfile<BlogProfile>();
        });

        _mapper = _configuration.CreateMapper();
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsBlogAndPublishesEvent()
    {
        // Arrange
        var blog = Blog.New("Test title", "Test desc").WithAuthor("John Doe").WithPublicationDate(DateTime.Now);
        var command = new CreateBlogCommand(blog.Title, blog.Content, blog.Author);

        var blogStorageMock = new Mock<IBlogRepository>();
        blogStorageMock.Setup(x => x.AddBlog(blog)).Returns(Task.CompletedTask);

        var publisherMock = new Mock<IPublisher>();

        var commandHandler = new CreateBlogCommandHandler(blogStorageMock.Object, _mapper);
        var commandHandler2 = new CreateBlogCommandHandler(blogStorageMock.Object, publisherMock.Object);

        // Act
        await commandHandler.Handle(command, CancellationToken.None);

        // Assert
        blogStorageMock.Verify(x => x.AddBlog(blog), Times.Once);
        publisherMock.Verify(x => x.Publish(It.IsAny<Blog>(), default), Times.Once);
    }
}