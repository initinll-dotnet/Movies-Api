using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

using Refit;

namespace Movies.Api.Sdk;


[Headers("Authorization: Bearer")]
public interface IMoviesApi
{
    [Get(ApiEndpoints.Movies.Get)]
    Task<MovieResponse> GetMovieAsync(string idOrSlug);

    [Get(ApiEndpoints.Movies.GetAll)]
    Task<IEnumerable<MovieResponse>> GetMoviesAsync();

    [Get(ApiEndpoints.Movies.Create)]
    Task<MovieResponse> CreateMovieAsync(CreateMovieRequest request);

    [Put(ApiEndpoints.Movies.Update)]
    Task<MovieResponse> UpdateMovieAsync(Guid id, UpdateMovieRequest request);

    [Delete(ApiEndpoints.Movies.Delete)]
    Task DeleteMovieAsync(Guid id, UpdateMovieRequest request);

    [Put(ApiEndpoints.Movies.Rate)]
    Task RateMovieAsync(Guid id, RateMovieRequest request);

    [Delete(ApiEndpoints.Movies.DeleteRatings)]
    Task DeleteRatingAsync(Guid id);
}
