using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SimpleBlog.Api.Controllers.Abstractions;

public class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;

    public BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public int UserId
    {
        get
        {
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserIdentifier")?.Value, out int loggedUserId);
            return loggedUserId;
        }
    }
}