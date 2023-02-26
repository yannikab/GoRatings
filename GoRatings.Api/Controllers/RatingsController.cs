using System.Text;

using GoRatings.Api.Contracts.Ratings;
using GoRatings.Services.Caching.Interfaces;
using GoRatings.Services.RatingCalculation.Interfaces;
using GoRatings.Services.RatingPersister.Exceptions;
using GoRatings.Services.RatingPersister.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GoRatings.Api.Controllers;

[ApiController]
[Route("[controller]")]
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
    public IActionResult CreateRating([FromBody] CreateRatingRequest request)
    {
        if (!ModelState.IsValid)
        {
            var sb = new StringBuilder();

            foreach (ModelStateEntry mse in ModelState.Values.Where(mse => mse != null))
                sb.AppendLine(string.Join(", ", mse.Errors.Select(e => e.ErrorMessage)));

            return BadRequest(sb.ToString());
        }

        IGivenRating gr = request.ToGivenRating();

        try
        {
            persisterService.Add(gr);
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

        return CreatedAtAction(nameof(GetRating), new { entityUid = gr.EntityUid }, gr);
    }

    [HttpGet("{entityUid:guid}")]
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

            var overallRatingResponse = overallRating.ToOveralRatingResponse(entityUid);

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
