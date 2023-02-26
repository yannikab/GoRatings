namespace GoRatings.Services.RatingCalculation.Interfaces;

public interface IRatingCalculationService
{
    IOverallRating CalculateOverallRating(IEnumerable<IConsideredRating> consideredRatings, int pastDays);
    Task<IOverallRating> CalculateOverallRatingAsync(IEnumerable<IConsideredRating> consideredRatings, int pastDays);
}
