using Microsoft.AspNetCore.Mvc;

using Movies.Application.Services;
using Movies.Minimal.Api.Auth;
using Movies.Minimal.Api.EndpointsConfig;

namespace Movies.Minimal.Api.Endpoints.Ratings.Routes;

public static class DeleteRatingEndpoint
{
    public const string Name = "DeleteRating";

    public static IEndpointRouteBuilder MapDeleteRating(this IEndpointRouteBuilder builder)
    {
        builder
            .MapDelete(ApiEndpoints.Movies.DeleteRatings, DeleteRating)
            .WithName(Name)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization()
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .WithOpenApi();

        return builder;
    }

    private static async Task<IResult> DeleteRating([FromRoute] Guid id,
        [FromServices] IRatingService ratingService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var userId = httpContext.GetUserId();
        var result = await ratingService.DeleteRatingAsync(id, userId!.Value, cancellationToken);
        return result ? Results.Ok() : Results.NotFound();
    }
}
