using GoRatings.Services.RatingCalculation.Interfaces;

namespace GoRatings.Services.RatingCalculation.Models;

public class OverallRatingFactory : IOverallRatingFactory
{
    public IOverallRating CreateOverallRating(DateTime calculatedDT, int consideredRatings, decimal rating)
    {
        return new OverallRating()
        {
            CalculatedDT = calculatedDT,
            ConsideredRatings = consideredRatings,
            Rating = rating,
        };
    }
}
