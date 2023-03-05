using GoRatings.Services.RatingCalculation.Interfaces;

namespace GoRatings.Services.RatingCalculation.Models;

public class ConsideredRatingFactory : IConsideredRatingFactory
{
    public IConsideredRating CreateConsideredRating(decimal rating, DateTime createdDT, bool isAnonymous)
    {
        return new ConsideredRating()
        {
            Rating = rating,
            CreatedDT = createdDT,
            IsAnonymous = isAnonymous
        };
    }
}
