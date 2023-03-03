using GoRatings.Services.RatingCalculation.Interfaces;

namespace GoRatings.Services.RatingCalculation;

public interface IRatingCalculationService
{
    IOverallRating CalculateOverallRating(IEnumerable<IConsideredRating> consideredRatings, DateTime referenceDT, int windowDays);
    Task<IOverallRating> CalculateOverallRatingAsync(IEnumerable<IConsideredRating> consideredRatings, DateTime referenceDT, int windowDays);
}
