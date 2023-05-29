using Movies.Minimal.Api.Auth;
using Movies.Minimal.Api.EndpointsConfig;

namespace Movies.Minimal.Api.Endpoints.Ratings.Routes;

public static class Ping4Endpoint
{
    public const string Name = "Ping4";

    public static IEndpointRouteBuilder MapPing4(this IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(ApiEndpoints.Ping.Ping4, Ping4)
            .WithName(Name)
            .Produces<string>(StatusCodes.Status200OK)
            .RequireAuthorization(policyNames: AuthConstants.JwtOrApiKeyPolicyName)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .HasApiVersion(2.0)
            .WithOpenApi();

        return builder;
    }

    private static IResult Ping4(CancellationToken cancellationToken)
    {
        return TypedResults.Ok("Pong from JwtOrApiKeyPolicy");
    }
}
