using Microsoft.AspNetCore.Mvc;

using Movies.Application.Services;
using Movies.Contracts.Responses;
using Movies.Minimal.Api.Auth;
using Movies.Minimal.Api.EndpointsConfig;

namespace Movies.Minimal.Api.Endpoints.Movies.Routes;

public static class DeleteMovieEndpoint
{
    public const string Name = "DeleteMovie";

    public static IEndpointRouteBuilder MapDeleteMovie(this IEndpointRouteBuilder builder)
    {
        builder
            .MapDelete(ApiEndpoints.Movies.Delete, DeleteMovie)
            .WithName(Name)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(policyNames: AuthConstants.AdminUserPolicyName)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .WithOpenApi();

        return builder;
    }

    private static async Task<IResult> DeleteMovie([FromRoute] Guid id,
        [FromServices] IMovieService movieService,
        CancellationToken cancellationToken)
    {
        var deleted = await movieService.DeleteByIdAsync(id, cancellationToken);

        if (!deleted)
        {
            return Results.NotFound();
        }

        return TypedResults.Ok();
    }
}
