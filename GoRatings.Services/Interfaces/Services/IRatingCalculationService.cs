using GoRatings.Services.Interfaces.Rating;

namespace GoRatings.Services.Interfaces.Services;

public interface IRatingCalculationService
{
    IOverallRating CalculateOverallRating(IEnumerable<IConsideredRating> consideredRatings, int pastDays);
    Task<IOverallRating> CalculateOverallRatingAsync(IEnumerable<IConsideredRating> consideredRatings, int pastDays);
}
