using Movies.Minimal.Api.Endpoints.Movies.Extension;
using Movies.Minimal.Api.Endpoints.Ratings.Extension;

namespace Movies.Minimal.Api.Endpoints;

public static class EndpointsExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapMovieEndpoints();
        builder.MapRatingEndpoints();
        return builder;
    }
}
