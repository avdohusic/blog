using Microsoft.AspNetCore.Mvc;

namespace SimpleBlog.Api.Controllers;

public class BaseController : ControllerBase
{
    public int UserId
    {
        get
        {
            int.TryParse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserIdentifier")?.Value, out int loggedUserId);
            return loggedUserId;
        }
    }
}
