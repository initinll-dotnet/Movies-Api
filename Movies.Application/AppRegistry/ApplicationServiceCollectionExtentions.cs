using Microsoft.Extensions.DependencyInjection;

using Movies.Application.Database;
using Movies.Application.Repositories;

namespace Movies.Application.AppRegistry;

public static class ApplicationServiceCollectionExtentions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //services.AddSingleton<IMovieRepository, InMemoryMovieRepository>();
        services.AddSingleton<IMovieRepository, MovieRepository>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services,
        string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ =>
            new NpgsqlConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();

        return services;
    }
}
