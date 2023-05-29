using Microsoft.AspNetCore.Mvc;

using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Minimal.Api.Auth;
using Movies.Minimal.Api.EndpointsConfig;
using Movies.Minimal.Api.Mapping;

namespace Movies.Minimal.Api.Endpoints.Movies.Routes;

public static class UpdateMovieEndpoint
{
    public const string Name = "UpdateMovie";

    public static IEndpointRouteBuilder MapUpdateMovie(this IEndpointRouteBuilder builder)
    {
        builder
            .MapPatch(ApiEndpoints.Movies.Update, UpdateMovie)
            .WithName(Name)
            .RequireAuthorization(policyNames: AuthConstants.TrustedMemberPolicyName);

        return builder;
    }

    private static async Task<IResult> UpdateMovie(Guid id,
        [FromBody] UpdateMovieRequest request,
        [FromServices] IMovieService movieService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var userId = httpContext.GetUserId();

        var movie = request.MapToMovie(id);
        var updatedMovie = await movieService.UpdateAsync(movie, userId, cancellationToken);

        if (updatedMovie is null)
        {
            return Results.NotFound();
        }

        var response = updatedMovie.MapToMovieResponse();
        return TypedResults.Ok(response);
    }
}
