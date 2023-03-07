namespace GoRatings.Services.RatingCalculation.Interfaces;

public interface IRatingCalculationService
{
    IOverallRating CalculateOverallRating(IEnumerable<IConsideredRating> consideredRatings, DateTime referenceDT, int windowDays);
    Task<IOverallRating> CalculateOverallRatingAsync(IEnumerable<IConsideredRating> consideredRatings, DateTime referenceDT, int windowDays);
}
