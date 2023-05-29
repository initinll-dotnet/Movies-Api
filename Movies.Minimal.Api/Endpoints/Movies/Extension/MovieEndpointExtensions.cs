using Movies.Minimal.Api.Endpoints.Movies.Routes;

namespace Movies.Minimal.Api.Endpoints.Movies.Extension;

public static class MovieEndpointExtensions
{
    public static IEndpointRouteBuilder MapMovieEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGetMovie();
        builder.MapCreateMovie();
        builder.MapGetAllMovies();
        builder.MapUpdateMovie();
        builder.MapDeleteMovie();
        return builder;
    }
}
