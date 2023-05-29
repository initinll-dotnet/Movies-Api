using Microsoft.AspNetCore.Mvc;

using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Minimal.Api.Auth;
using Movies.Minimal.Api.EndpointsConfig;
using Movies.Minimal.Api.Mapping;

namespace Movies.Minimal.Api.Endpoints.Movies.Routes;

public static class CreateMovieEndpoint
{
    public const string Name = "CreateMovie";

    public static IEndpointRouteBuilder MapCreateMovie(this IEndpointRouteBuilder builder)
    {
        builder
            .MapPost(ApiEndpoints.Movies.Create, CreateMovie)
            .WithName(Name)
            .RequireAuthorization(policyNames: AuthConstants.TrustedMemberPolicyName);

        return builder;
    }

    private static async Task<IResult> CreateMovie([FromBody] CreateMovieRequest request,
        [FromServices] IMovieService movieService,
        CancellationToken cancellationToken)
    {
        var movie = request.MapToMovie();

        await movieService.CreateAsync(movie, cancellationToken);

        var movieResponse = movie.MapToMovieResponse();

        return TypedResults.CreatedAtRoute(movieResponse, GetMovieEndpoint.Name, new { idOrSlug = movieResponse.Id });
    }
}
