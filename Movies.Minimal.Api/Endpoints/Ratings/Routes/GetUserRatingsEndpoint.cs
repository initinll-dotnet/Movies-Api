using Microsoft.AspNetCore.Mvc;

using Movies.Application.Services;
using Movies.Minimal.Api.Auth;
using Movies.Minimal.Api.EndpointsConfig;
using Movies.Minimal.Api.Mapping;

namespace Movies.Minimal.Api.Endpoints.Ratings.Routes;

public static class GetUserRatingsEndpoint
{
    public const string Name = "GetUserRatings";

    public static IEndpointRouteBuilder MapGetUserRatings(this IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(ApiEndpoints.Ratings.GetUserRatings, GetUserRatings)
            .WithName(Name)
            .RequireAuthorization();

        return builder;
    }

    private static async Task<IResult> GetUserRatings([FromServices] IRatingService ratingService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var userId = httpContext.GetUserId();
        var ratings = await ratingService.GetRatingsForUserAsync(userId!.Value, cancellationToken);
        var ratingsResponse = ratings.MapToMovieRatingResponse();
        return TypedResults.Ok(ratingsResponse);
    }
}
