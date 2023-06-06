using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Api.Controllers.Abstractions;
using SimpleBlog.Application.Dtos;
using SimpleBlog.Application.Features.Users.Commands;
using SimpleBlog.Domain.Shared;

namespace SimpleBlog.Api.Controllers;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/users")]
public class UserController : BaseController
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a user
    /// </summary>
    /// <response code="200">Returns newly created user</response>
    /// <response code="400">One or more properties are not valid</response>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register([FromBody] UserRegistrationInputModel model)
    {
        var command = new CreateUserCommand(model.Username, model.Password, model.Email);
        var result = await _mediator.Send(command);

        // In fully implemented REST API this should be CreatedAtAction response, not Ok.
        return Ok(result);
    }
}