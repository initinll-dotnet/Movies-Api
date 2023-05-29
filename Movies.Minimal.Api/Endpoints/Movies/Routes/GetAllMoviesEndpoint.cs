using Microsoft.AspNetCore.Mvc;

using Movies.Application.Services;
using Movies.Contracts.Responses;
using Movies.Minimal.Api.Auth;
using Movies.Minimal.Api.EndpointsConfig;
using Movies.Minimal.Api.Mapping;

namespace Movies.Minimal.Api.Endpoints.Movies.Routes;

public static class GetAllMoviesEndpoint
{
    public const string Name = "GetMovies";

    public static IEndpointRouteBuilder MapGetAllMovies(this IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(ApiEndpoints.Movies.GetAll, GetMovies)
            .WithName(Name)
            .Produces<MovieResponse>(StatusCodes.Status200OK)
            .AllowAnonymous()
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .WithOpenApi();

        return builder;
    }

    private static async Task<IResult> GetMovies([FromServices] IMovieService movieService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var userId = httpContext.GetUserId();

        var movies = await movieService.GetAllAsync(userId, cancellationToken);

        var response = movies.MapToMovieResponse();
        return TypedResults.Ok(response);
    }
}
