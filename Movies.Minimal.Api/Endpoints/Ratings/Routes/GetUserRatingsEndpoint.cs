using Microsoft.AspNetCore.Mvc;

using Movies.Application.Services;
using Movies.Contracts.Responses;
using Movies.Minimal.Api.Auth;
using Movies.Minimal.Api.EndpointsConfig;
using Movies.Minimal.Api.Mapping;

namespace Movies.Minimal.Api.Endpoints.Ratings.Routes;

public static class GetUserRatingsEndpoint
{
    public const string Name1 = "GetUserRatingsV1";
    public const string Name2 = "GetUserRatingsV2";

    public static IEndpointRouteBuilder MapGetUserRatingsV1(this IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(ApiEndpoints.Ratings.GetUserRatings, GetUserRatings)
            .WithName(Name1)
            .Produces<IEnumerable<MovieRatingResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization()
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .WithOpenApi();

        return builder;
    }

    public static IEndpointRouteBuilder MapGetUserRatingsV2(this IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(ApiEndpoints.Ratings.GetUserRatings, GetUserRatings)
            .WithName(Name2)
            .Produces<IEnumerable<MovieRatingResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization()
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(2.0);

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
