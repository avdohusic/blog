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
[Route("api/blogs")]
[Produces("application/json")]
public class BlogController : BaseController
{

    public BlogController(IMediator mediator) : base(mediator)
    {
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
    /// <response code="401">Unauthorized user</response>
    /// <response code="404">The blog was not found</response>
    /// <returns></returns>
    [HttpGet("{blogId}")]
    [ProducesResponseType(typeof(BlogDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status401Unauthorized)]
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
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Missing required permission</response>
    /// <returns></returns>
    [HttpGet("export")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(typeof(File), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ExportAllBlogs()
    {
        var memoryStream = await _mediator.Send(new ExportAllBlogsQuery());
        var fileName = $"{GlobalConstants.BlogTableName}_{DateTime.Now.ToFileTime()}.xlsx";
        HttpContext.Response.Headers.Add(GlobalConstants.FileNameHeaderKey, fileName);
        return File(memoryStream, GlobalConstants.SpreadsheetmlContentType, fileName);
    }

    /// <summary>
    /// Import Blogs from Excel file
    /// </summary>
    /// <response code="204">The import of the blog was successful</response>
    /// <response code="400">Excel sheet is not in valid format</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Missing required permission</response>
    /// <returns></returns>
    [HttpPost("import")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(typeof(BlogDto), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ImportBlogs(IFormFile file)
    {
        await _mediator.Send(new ImportBlogCommand(file?.OpenReadStream()));

        return NoContent();
    }

    /// <summary>
    /// Create a blog
    /// </summary>
    /// <response code="201">Returns newly created blog</response>
    /// <response code="400">One or more properties are not valid</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Missing required permission</response>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Publisher")]
    [ProducesResponseType(typeof(BlogDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status403Forbidden)]
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
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Missing required permission</response>
    /// <response code="404">The blog was not found</response>
    /// <returns></returns>
    [HttpPut("{blogId}")]
    [Authorize(Roles = "Publisher")]
    [ProducesResponseType(typeof(BlogDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status403Forbidden)]
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
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Missing required permission</response>
    /// <response code="404">The blog was not found</response>
    /// <returns></returns>
    [HttpDelete("{blogId}")]
    [Authorize(Roles = "Publisher")]
    [ProducesResponseType(typeof(BlogDto), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBlog(Guid blogId)
    {
        var command = new DeleteBlogCommand(blogId);
        await _mediator.Send(command);

        return NoContent();
    }
}