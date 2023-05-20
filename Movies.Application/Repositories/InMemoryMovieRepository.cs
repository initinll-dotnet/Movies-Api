using Movies.Application.Models;

namespace Movies.Application.Repositories;

public class InMemoryMovieRepository : IMovieRepository
{
    private readonly List<Movie> _movies = new();

    public Task<bool> CreateAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        _movies.Add(movie);
        return Task.FromResult(true);
    }

    public Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken cancellationToken = default)
    {
        var movie = _movies.SingleOrDefault(x => x.Id == id);
        return Task.FromResult(movie);
    }

    public Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken cancellationToken = default)
    {
        var movie = _movies.SingleOrDefault(x => x.Slug == slug);
        return Task.FromResult(movie);
    }

    public Task<bool> UpdateAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        var movieIndex = _movies.FindIndex(x => x.Id == movie.Id);

        if (movieIndex == -1)
        {
            return Task.FromResult(false);
        }

        _movies[movieIndex] = movie;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var removedCount = _movies.RemoveAll(x => x.Id == id);
        var movieRemoved = removedCount > 0;
        return Task.FromResult(movieRemoved);
    }

    public Task<IEnumerable<Movie>> GetAllAsync(Guid? userId = default, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_movies.AsEnumerable());
    }

    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_movies.Count > 0);
    }
}
