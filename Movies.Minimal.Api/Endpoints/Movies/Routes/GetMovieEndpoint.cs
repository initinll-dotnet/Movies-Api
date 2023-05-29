using Microsoft.AspNetCore.Mvc;

using Movies.Application.Services;
using Movies.Contracts.Responses;
using Movies.Minimal.Api.Auth;
using Movies.Minimal.Api.EndpointsConfig;
using Movies.Minimal.Api.Mapping;

namespace Movies.Minimal.Api.Endpoints.Movies.Routes;

public static class GetMovieEndpoint
{
    public const string Name = "GetMovie";

    public static IEndpointRouteBuilder MapGetMovie(this IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(ApiEndpoints.Movies.Get, GetMovie)
            .WithName(Name)
            .Produces<MovieResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status404NotFound)
            .AllowAnonymous()
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .WithOpenApi();

        return builder;
    }

    private static async Task<IResult> GetMovie([FromRoute] string idOrSlug,
        [FromServices] IMovieService movieService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var userId = httpContext.GetUserId();

        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await movieService.GetByIdAsync(id, userId, cancellationToken)
            : await movieService.GetBySlugAsync(idOrSlug, userId, cancellationToken);

        if (movie is null)
        {
            return Results.NotFound();
        }

        var response = movie.MapToMovieResponse();
        return TypedResults.Ok(response);
    }
}
