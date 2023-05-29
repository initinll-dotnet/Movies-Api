using Microsoft.AspNetCore.Mvc;

using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Minimal.Api.Auth;
using Movies.Minimal.Api.EndpointsConfig;

namespace Movies.Minimal.Api.Endpoints.Ratings.Routes;

public static class RateMovieEndpoint
{
    public const string Name = "RateMovie";

    public static IEndpointRouteBuilder MapRateMovie(this IEndpointRouteBuilder builder)
    {
        builder
            .MapPost(ApiEndpoints.Movies.Rate, RateMovie)
            .WithName(Name)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization()
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .WithOpenApi();

        return builder;
    }

    private static async Task<IResult> RateMovie([FromRoute] Guid id,
        [FromBody] RateMovieRequest request,
        [FromServices] IRatingService ratingService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var userId = httpContext.GetUserId();
        var result = await ratingService.RateMovieAsync(id, request.Rating, userId!.Value, cancellationToken);
        return result ? Results.Ok() : Results.NotFound();
    }
}
