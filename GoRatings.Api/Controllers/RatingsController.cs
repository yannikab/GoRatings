using GoRatings.Api.Contracts.Ratings;
using GoRatings.Services.Exceptions.Entity;
using GoRatings.Services.Interfaces.Rating;
using GoRatings.Services.Interfaces.Services;

using Microsoft.AspNetCore.Mvc;

namespace GoRatings.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class RatingsController : ControllerBase
{
    private readonly IRatingPersisterService persisterService;
    private readonly IRatingCalculationService calculationService;
    private readonly ICachingService<Guid, OverallRatingResponse> cachingService;

    public RatingsController(
        IRatingPersisterService persisterService,
        IRatingCalculationService calculationService,
        ICachingService<Guid, OverallRatingResponse> cachingService)
    {
        this.persisterService = persisterService;
        this.calculationService = calculationService;
        this.cachingService = cachingService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateRating([FromBody] CreateRatingRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrors());

        IGivenRating givenRating = request.ToGivenRating();

        try
        {
            var storedRating = persisterService.Add(givenRating);

            return CreatedAtAction(nameof(GetRating), new { entityUid = storedRating.EntityUid }, storedRating);
        }
        catch (Exception ex) when (ex is EntityDoesNotExistException || ex is EntityInvalidException || ex is EntityUidTypeMismatchException)
        {
            Console.WriteLine(ex.Message);

            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return StatusCode(500);
        }
    }

    [HttpGet("{entityUid:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetRating(Guid entityUid)
    {
        try
        {
            if (cachingService.TryGetValue(entityUid, out var cachedRatingResponse))
                return Ok(cachedRatingResponse);

            int pastDays = Settings.Instance.PastDays;

            var consideredRatings = persisterService.GetWithinPastDays(entityUid, pastDays).Select(sr => sr.ToConsideredRating());

            var overallRating = calculationService.CalculateOverallRating(consideredRatings, pastDays);

            if (!(overallRating.ConsideredRatings > 0))
                return NotFound(entityUid);

            var overallRatingResponse = overallRating.ToOverallRatingResponse(entityUid);

            cachingService.Add(entityUid, overallRatingResponse);

            return Ok(overallRatingResponse);
        }
        catch (Exception ex) when (ex is EntityDoesNotExistException || ex is EntityInvalidException)
        {
            Console.WriteLine(ex.Message);

            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return StatusCode(500);
        }
    }
}
