using Movies.Minimal.Api.Endpoints.Ratings.Routes;

namespace Movies.Minimal.Api.Endpoints.Ratings.Extension;

public static class PingEndpointExtensions
{
    public static IEndpointRouteBuilder MapRatingEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapRateMovie();
        builder.MapDeleteRating();
        builder.MapGetUserRatingsV1();
        //builder.MapGetUserRatingsV2();

        return builder;
    }
}
