using AutoMapper;
using Moq;
using SimpleBlog.Application.Common.Mappings;
using SimpleBlog.Application.Dtos;
using SimpleBlog.Application.Features.Blogs.Commands;
using SimpleBlog.Domain.Repositories;

namespace SimpleBlog.Application.UnitTests.Features.Blogs;
public class CreateBlogCommandHandlerShould
{
    private readonly CreateBlogCommandHandler _sut;
    private readonly Mock<IBlogRepository> _mockBlogRepository;

    public CreateBlogCommandHandlerShould()
    {
        _mockBlogRepository = new Mock<IBlogRepository>();

        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<BlogProfile>());
        var mapper = mapperConfiguration.CreateMapper();

        _sut = new CreateBlogCommandHandler(blogRepository: _mockBlogRepository.Object, mapper: mapper);
    }

    [Fact]
    public async Task CreateBlogEntity()
    {
        // Arrange
        var command = new CreateBlogCommand("Test title", "Test desc", "John Doe");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<BlogDto>();
    }
}