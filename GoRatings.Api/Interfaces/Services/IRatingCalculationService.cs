using GoRatings.Api.Interfaces.Rating;

namespace GoRatings.Api.Interfaces.Services.RatingCalculation;

public interface IRatingCalculationService
{
    IOverallRating CalculateOverallRating(IEnumerable<IConsideredRating> consideredRatings, DateTime referenceDT, int windowDays);
    Task<IOverallRating> CalculateOverallRatingAsync(IEnumerable<IConsideredRating> consideredRatings, DateTime referenceDT, int windowDays);
}
