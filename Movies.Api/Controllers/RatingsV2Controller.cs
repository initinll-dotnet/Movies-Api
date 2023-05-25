using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

using Movies.Api.Auth;

using Movies.Api.EndpointsConfig;
using Movies.Api.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers;

[ApiController]
[ApiVersion("2.0")]
public class RatingsV2Controller : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingsV2Controller(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [Authorize]
    [EnableQuery]
    [HttpGet(ApiEndpoints.Ratings.GetUserRatings)]
    [ProducesResponseType(typeof(IEnumerable<MovieRatingResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserRatings(CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();
        var ratings = await _ratingService.GetRatingsForUserAsync(userId!.Value, cancellationToken);
        var ratingsResponse = ratings.MapToMovieRatingResponse();
        return Ok(ratingsResponse);
    }
}
