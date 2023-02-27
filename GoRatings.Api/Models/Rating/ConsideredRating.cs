using GoRatings.Api.Exceptions.Rating;
using GoRatings.Api.Interfaces.Rating;

namespace GoRatings.Api.Models.Rating;

public class ConsideredRating : IConsideredRating
{
    public decimal Rating { get; set; }
    public DateTime CreatedDT { get; set; }
    public bool IsAnonymous { get; set; }

    public decimal CalculateRating(DateTime referenceDT, int windowDays)
    {
        if (!(windowDays > 0))
            throw new ArgumentOutOfRangeException(nameof(windowDays));

        if (CreatedDT > referenceDT)
            throw new RatingCalculationException();

        decimal rating = 100m;

        rating *= 0.2m * Rating;

        rating *= 1.0m - Math.Min((decimal)(referenceDT - CreatedDT).TotalDays, windowDays) / windowDays;

        if (IsAnonymous)
            rating *= 0.1m;

        return rating;
    }
}
