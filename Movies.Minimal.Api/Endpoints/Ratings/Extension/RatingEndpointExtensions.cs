using Movies.Minimal.Api.Endpoints.Ratings.Routes;

namespace Movies.Minimal.Api.Endpoints.Ratings.Extension;

public static class RatingEndpointExtensions
{
    public static IEndpointRouteBuilder MapRatingEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapRateMovie();
        builder.MapDeleteRating();
        builder.MapGetUserRatings();

        return builder;
    }
}
