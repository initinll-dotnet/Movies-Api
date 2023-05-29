using Microsoft.AspNetCore.Mvc;

using Movies.Minimal.Api.Auth;
using Movies.Minimal.Api.EndpointsConfig;

namespace Movies.Minimal.Api.Endpoints.Ratings.Routes;

public static class Ping5Endpoint
{
    public const string Name = "Ping5";

    public static IEndpointRouteBuilder MapPing5(this IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(ApiEndpoints.Ping.Ping5, Ping5)
            .WithMetadata(new ServiceFilterAttribute(typeof(ApiKeyAuthFilter)))
            .WithName(Name)
            .Produces<string>(StatusCodes.Status200OK)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .HasApiVersion(2.0)
            .WithOpenApi();

        return builder;
    }

    private static IResult Ping5(CancellationToken cancellationToken)
    {
        return TypedResults.Ok("Pong from ApiKeyAuthFilter");
    }
}
