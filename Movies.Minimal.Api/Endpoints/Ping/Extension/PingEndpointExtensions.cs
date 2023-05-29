using Movies.Minimal.Api.Endpoints.Ratings.Routes;

namespace Movies.Minimal.Api.Endpoints.Ping.Extension;

public static class PingEndpointExtensions
{
    public static IEndpointRouteBuilder MapPingEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPing1();
        builder.MapPing2();
        builder.MapPing3();
        builder.MapPing4();
        builder.MapPing5();

        return builder;
    }
}
