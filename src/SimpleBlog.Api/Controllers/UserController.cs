using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Api.Controllers.Abstractions;
using SimpleBlog.Api.Helpers;
using SimpleBlog.Application.Dtos;
using SimpleBlog.Application.Features.Users.Commands;
using SimpleBlog.Application.Features.Users.Queries;
using SimpleBlog.Domain.Shared;
using System.Security.Claims;

namespace SimpleBlog.Api.Controllers;

[Authorize]
[ApiController]
[Produces("application/json")]
[Route("api/users")]
public class UserController : BaseController
{
    public UserController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Log in a user
    /// </summary>
    /// <response code="200">Returns JWT token</response>
    /// <response code="400">One or more properties are not valid</response>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UserLoginInputModel model)
    {
        var command = new GetUserByUsernameAndPasswordQuery(model.Username, model.Password);
        var user = await _mediator.Send(command);

        if (user is null)
        {
            return NotFound();
        }

        var roles = await _mediator.Send(new GetUserRolesByUsernameQuery(model.Username));
        string token = new JwtTokenBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create("simpleblog-secret-key"))
                    .AddSubject(user.Email)
                    .AddIssuer("SimpleBlog.Api.Security.Bearer")
                    .AddAudience("SimpleBlog.Api.Security.Bearer")
                    .AddClaim("UserIdentifier", user.Id.ToString())
                    .AddClaim(ClaimTypes.Name, model.Username)
                    .AddClaim(ClaimTypes.Role, roles)
                    .Build();

        return Ok(new
        {
            Result = token
        });
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