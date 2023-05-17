namespace Movies.Contracts.Responses;

public record MoviesResponse
{
    public IEnumerable<MovieResponse> Items { get; init; } = Enumerable.Empty<MovieResponse>();
}
