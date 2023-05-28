
using Microsoft.Extensions.DependencyInjection;

using Movies.Api.Sdk;
using Movies.Api.Sdk.Consumer;

using Refit;

using System.Text.Json;

//var moviesApi = RestService.For<IMoviesApi>("http://localhost:5010");

var services = new ServiceCollection();

services
    .AddHttpClient()
    .AddSingleton<AuthTokenProvider>()
    .AddRefitClient<IMoviesApi>(serviceProvider => 
    new RefitSettings 
    { 
        AuthorizationHeaderValueGetter = async () => await serviceProvider.GetRequiredService<AuthTokenProvider>().GetTokenAsync()
    })
    .ConfigureHttpClient(x => x.BaseAddress = new Uri("http://localhost:5010"));

var provider = services.BuildServiceProvider();

var moviesApi = provider.GetRequiredService<IMoviesApi>();

try
{
    var movie = await moviesApi.GetMovieAsync("don-2000");
    var movies = await moviesApi.GetMoviesAsync();

    Console.WriteLine(JsonSerializer.Serialize(movie));
    Console.WriteLine(JsonSerializer.Serialize(movies));
}
catch (Exception ex)
{
    Console.WriteLine($"Exception : {ex.InnerException}");
}