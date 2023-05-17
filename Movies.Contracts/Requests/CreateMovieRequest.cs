﻿namespace Movies.Contracts.Requests;

public record CreateMovieRequest
{
    public required string Title { get; init; }
    public required int YearOfRelease { get; init; }
    public required IEnumerable<string> Genres { get; init; } = Enumerable.Empty<string>();
}
