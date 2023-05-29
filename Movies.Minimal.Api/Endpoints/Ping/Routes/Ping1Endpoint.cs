using Movies.Minimal.Api.EndpointsConfig;

namespace Movies.Minimal.Api.Endpoints.Ratings.Routes;

public static class Ping1Endpoint
{
    public const string Name = "Ping1";

    public static IEndpointRouteBuilder MapPing1(this IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(ApiEndpoints.Ping.Ping1, Ping1)
            .WithName(Name)
            .Produces<string>(StatusCodes.Status200OK)
            .AllowAnonymous()
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .HasApiVersion(2.0)
            .WithOpenApi();

        return builder;
    }

    private static IResult Ping1(CancellationToken cancellationToken)
    {
        return TypedResults.Ok("Pong from AllowAnonymous");
    }
}
