using GoRatings.Services.RatingCalculation.Exceptions;
using GoRatings.Services.RatingCalculation.Interfaces;
using GoRatings.Services.RatingCalculation.Models;

namespace GoRatings.Services.RatingCalculation;

public class RatingCalculationService : IRatingCalculationService
{
    public IOverallRating CalculateOverallRating(IEnumerable<IConsideredRating> consideredRatings, DateTime referenceDT, int windowDays)
    {
        if (!(windowDays > 0))
            throw new RatingCalculationException();

        int count = 0;
        decimal rating = 0m;

        foreach (var cr in consideredRatings)
        {
            if (!cr.IsValid())
                throw new RatingCalculationException();

            rating += cr.EffectiveRating(referenceDT, windowDays);

            count++;
        }

        if (count > 0)
        {
            rating /= count;

            rating /= 20;

            rating = Math.Truncate(100 * rating) / 100;
        }

        var overallRating = new OverallRating()
        {
            CalculatedDT = DateTime.UtcNow,
            ConsideredRatings = count,
            Rating = rating,
        };

        if (!overallRating.IsValid())
            throw new OverallRatingInvalidException(overallRating.Rating);

        return overallRating;
    }

    public async Task<IOverallRating> CalculateOverallRatingAsync(IEnumerable<IConsideredRating> consideredRatings, DateTime referenceDT, int windowDays)
    {
        return await Task.Run(() => CalculateOverallRating(consideredRatings, referenceDT, windowDays));
    }
}
