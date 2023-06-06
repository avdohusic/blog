using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using SimpleBlog.Application.Features.Users.Queries.GetUserByUsernameAndPassword;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace SimpleBlog.Api.Helpers;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IMediator _mediator;

    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IMediator mediator)
        : base(options, logger, encoder, clock)
    {
        _mediator = mediator;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string username = null;
        int? userIdentifier = null;
        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
            username = credentials.FirstOrDefault();
            var password = credentials.LastOrDefault();

            var query = new GetUserByUsernameAndPasswordQuery(username, password);
            var user = await _mediator.Send(query);
            if (user is null)
            {
                throw new ArgumentException("Invalid credentials");
            }
            userIdentifier = user.Id;
        }
        catch (Exception ex)
        {
            return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");
        }

        var claims = new[]
        {
            new Claim("UserIdentifier", userIdentifier.ToString()),
            new Claim(ClaimTypes.Name, username)
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

}
