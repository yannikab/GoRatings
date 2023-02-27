using GoRatings.Api.Interfaces.Rating;

namespace GoRatings.Api.Interfaces.Services;

public interface IRatingCalculationService
{
    IOverallRating CalculateOverallRating(IEnumerable<IConsideredRating> consideredRatings, int pastDays);
    Task<IOverallRating> CalculateOverallRatingAsync(IEnumerable<IConsideredRating> consideredRatings, int pastDays);
}
