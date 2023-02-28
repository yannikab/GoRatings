using GoRatings.Api.Exceptions.Rating;
using GoRatings.Api.Interfaces.Rating;
using GoRatings.Api.Interfaces.Services.RatingCalculation;
using GoRatings.Api.Models.Rating;

namespace GoRatings.Api.Services.RatingCalculation;

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
            cr.Validate();

            try
            {
                rating += cr.EffectiveRating(referenceDT, windowDays);
            }
            catch (RatingCalculationException)
            {
                continue;
            }

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
            throw new RatingValueInvalidException(overallRating.Rating);

        return overallRating;
    }

    public async Task<IOverallRating> CalculateOverallRatingAsync(IEnumerable<IConsideredRating> consideredRatings, DateTime referenceDT, int windowDays)
    {
        return await Task.Run(() => CalculateOverallRating(consideredRatings, referenceDT, windowDays));
    }
}
