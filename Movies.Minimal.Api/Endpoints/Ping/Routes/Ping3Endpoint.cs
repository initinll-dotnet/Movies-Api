using Movies.Minimal.Api.Auth;
using Movies.Minimal.Api.EndpointsConfig;

namespace Movies.Minimal.Api.Endpoints.Ratings.Routes;

public static class Ping3Endpoint
{
    public const string Name = "Ping3";

    public static IEndpointRouteBuilder MapPing3(this IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(ApiEndpoints.Ping.Ping3, Ping3)
            .WithName(Name)
            .Produces<string>(StatusCodes.Status200OK)
            .RequireAuthorization(policyNames: AuthConstants.AdminUserPolicyName)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .HasApiVersion(2.0)
            .WithOpenApi();

        return builder;
    }

    private static IResult Ping3(CancellationToken cancellationToken)
    {
        return TypedResults.Ok("Pong from AdminUserPolicy");
    }
}
