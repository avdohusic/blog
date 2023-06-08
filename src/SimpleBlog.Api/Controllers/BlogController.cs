using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Api.Controllers.Abstractions;
using SimpleBlog.Application.Dtos;
using SimpleBlog.Application.Features.Blogs.Commands;
using SimpleBlog.Application.Features.Blogs.Queries;
using SimpleBlog.Domain.Constants;
using SimpleBlog.Domain.Shared;

namespace SimpleBlog.Api.Controllers;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/blogs")]
public class BlogController : BaseController
{
    private readonly IMediator _mediator;

    public BlogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns all blogs
    /// </summary>
    /// <response code="200">Returns list of blogs</response>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<BlogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllBlogs()
    {
        var query = new GetAllBlogsQuery();
        var blogs = await _mediator.Send(query);

        return Ok(blogs);
    }

    /// <summary>
    /// Returns blog by id
    /// </summary>
    /// <response code="200">Returns specific blog by id</response>
    /// <returns></returns>
    [HttpGet("{blogId}")]
    [ProducesResponseType(typeof(BlogDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBlogById(Guid blogId)
    {
        var query = new GetBlogByIdQuery(blogId);
        return Ok(await _mediator.Send(query));
    }

    /// <summary>
    /// Returns exported blogs in an Excel file
    /// </summary>
    /// <response code="200">Returns Excel file</response>
    /// <returns></returns>
    [HttpGet("export")]
    [ProducesResponseType(typeof(File), StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportAllBlogs()
    {
        return File(await _mediator.Send(new ExportAllBlogsQuery()),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"{GlobalConstants.BlogTableName}_{DateTime.Now.ToFileTime()}.xlsx");
    }

    /// <summary>
    /// Create a blog
    /// </summary>
    /// <response code="201">Returns newly created blog</response>
    /// <response code="400">One or more properties are not valid</response>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(BlogDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBlog(CreateBlogCommand command)
    {
        var result = await _mediator.Send(command);

        return CreatedAtAction("GetBlogById", new { blogId = result.BlogId }, result);
    }

    /// <summary>
    /// Update a blog
    /// </summary>
    /// <response code="200">Returns updated blog</response>
    /// <response code="400">One or more properties are not valid</response>
    /// <response code="404">The blog was not found</response>
    /// <returns></returns>
    [HttpPut("{blogId}")]
    [ProducesResponseType(typeof(BlogDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBlog(Guid blogId, UpdateBlogCommand command)
    {
        command.BlogId = blogId;
        var blog = await _mediator.Send(command);

        return Ok(blog);
    }

    /// <summary>
    /// Delete a blog
    /// </summary>
    /// <response code="204">The blog was successfully deleted</response>
    /// <response code="404">The blog was not found</response>
    /// <returns></returns>
    [HttpDelete("{blogId}")]
    [ProducesResponseType(typeof(BlogDto), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBlog(Guid blogId)
    {
        var command = new DeleteBlogCommand(blogId);
        await _mediator.Send(command);

        return NoContent();
    }
}