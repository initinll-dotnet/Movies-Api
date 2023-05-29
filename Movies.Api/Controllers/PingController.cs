using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

using Movies.Mvc.Api.Auth;
using Movies.Mvc.Api.EndpointsConfig;

namespace Movies.Mvc.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class PingController : ControllerBase
{
    [AllowAnonymous]
    [EnableQuery]
    [HttpGet(ApiEndpoints.Ping.Ping1)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult Ping1(CancellationToken cancellationToken)
    {
        return Ok("Pong from AllowAnonymous");
    }

    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [EnableQuery]
    [HttpGet(ApiEndpoints.Ping.Ping2)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult Ping2(CancellationToken cancellationToken)
    {
        return Ok("Pong from TrustedMemberPolicy");
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [EnableQuery]
    [HttpGet(ApiEndpoints.Ping.Ping3)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult Ping3(CancellationToken cancellationToken)
    {
        return Ok("Pong from AdminUserPolicy");
    }

    [Authorize(AuthConstants.JwtOrApiKeyPolicyName)]
    [EnableQuery]
    [HttpGet(ApiEndpoints.Ping.Ping4)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult Ping4(CancellationToken cancellationToken)
    {
        return Ok("Pong from JwtOrApiKeyPolicy");
    }

    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    [EnableQuery]
    [HttpGet(ApiEndpoints.Ping.Ping5)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult Ping5(CancellationToken cancellationToken)
    {
        return Ok("Pong from ApiKeyAuthFilter");
    }
}
