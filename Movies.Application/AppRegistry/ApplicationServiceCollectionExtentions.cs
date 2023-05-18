using Microsoft.Extensions.DependencyInjection;

using Movies.Application.Repositories;

namespace Movies.Application.AppRegistry;

public static class ApplicationServiceCollectionExtentions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, MovieRepository>();

        return services;
    }
}
