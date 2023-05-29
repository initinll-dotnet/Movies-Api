using Microsoft.AspNetCore.Authorization;

namespace Movies.Mvc.Api.Auth;

public class AdminAuthRequirement : IAuthorizationHandler, IAuthorizationRequirement
{
    private readonly string _apikey;

    public AdminAuthRequirement(string apikey)
    {
        _apikey = apikey;
    }

    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (context.User.HasClaim(AuthConstants.AdminUserClaimName, "true") ||
            context.User.HasClaim(AuthConstants.TrustedMemberClaimName, "true"))
        {
            context.Succeed(this);
            return Task.CompletedTask;
        }

        var httpContext = context.Resource as HttpContext;
        if (httpContext is null)
        {
            return Task.CompletedTask;
        }

        if (!httpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName,
            out var extractedApiKey))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        if (_apikey != extractedApiKey)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        context.Succeed(this);
        return Task.CompletedTask;
    }
}
