using GoRatings.Api.Contracts.Ratings;
using GoRatings.Services.Caching.Interfaces;
using GoRatings.Services.RatingCalculation.Interfaces;
using GoRatings.Services.RatingPersister.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace GoRatings.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RatingsController : ControllerBase
{
    private readonly IGivenRatingFactory givenRatingFactory;
    private readonly IConsideredRatingFactory consideredRatingFactory;

    private readonly IRatingPersisterService ratingPersisterService;
    private readonly IRatingCalculationService ratingCalculationService;

    private readonly ICachingService<Guid, OverallRatingResponse> cachingService;

    public RatingsController(
        IGivenRatingFactory givenRatingFactory,
        IConsideredRatingFactory consideredRatingFactory,
        IRatingPersisterService ratingPersisterService,
        IRatingCalculationService ratingCalculationService,
        ICachingService<Guid, OverallRatingResponse> cachingService)
    {
        this.givenRatingFactory = givenRatingFactory;
        this.consideredRatingFactory = consideredRatingFactory;
        this.ratingPersisterService = ratingPersisterService;
        this.ratingCalculationService = ratingCalculationService;
        this.cachingService = cachingService;
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Adds a new rating.", Description = "Adds a new five-star style rating, for an entity of a specified type. A rater entity can be specified, otherwise the rating is created as anonymous.")]
    [SwaggerResponse(201, "Rating added successfully.", Type = typeof(CreateRatingResponse))]
    [SwaggerResponse(400, "Invalid request data.")]
    [SwaggerResponse(500, "An error has occurred.")]
    public async Task<IActionResult> CreateRating([FromBody][SwaggerRequestBody("Rating creation request.", Required = true)] CreateRatingRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrors());

        IGivenRating givenRating = request.ToGivenRating(givenRatingFactory);

        var storedRating = await ratingPersisterService.AddAsync(givenRating);

        var createdRatingResponse = storedRating.ToCreateRatingResponse();

        return CreatedAtAction(nameof(GetRating), new { entityUid = createdRatingResponse.EntityUid }, createdRatingResponse);
    }

    [HttpGet("{entityUid:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Gets the overall rating of an entity.", Description = "Returns the calculated overall rating for the entity with the specified unique id. The overall rating is determined by active ratings added for the entity in the past, if the entity is active.")]
    [SwaggerResponse(200, "Overall rating calculated successfully.", Type = typeof(OverallRatingResponse))]
    [SwaggerResponse(400, "Invalid request data.")]
    [SwaggerResponse(404, "No ratings found for the given unique id.")]
    [SwaggerResponse(500, "An error has occurred.")]
    public async Task<IActionResult> GetRating([SwaggerParameter("The unique id of the entity for which the overall rating is calculated.", Required = true)] Guid entityUid)
    {
        if (cachingService.TryGetValue(entityUid, out var cachedRatingResponse))
            return Ok(cachedRatingResponse);

        int pastDays = Settings.Instance.PastDays;

        var storedRatings = await ratingPersisterService.GetWithinPastDaysAsync(entityUid, pastDays);

        var consideredRatings = storedRatings.Select(sr => sr.ToConsideredRating(consideredRatingFactory));

        var overallRating = await ratingCalculationService.CalculateOverallRatingAsync(consideredRatings, DateTime.UtcNow, pastDays);

        if (!(overallRating.ConsideredRatings > 0))
            return NotFound(entityUid);

        var overallRatingResponse = overallRating.ToOverallRatingResponse(entityUid);

        cachingService.Add(entityUid, overallRatingResponse);

        return Ok(overallRatingResponse);
    }
}
