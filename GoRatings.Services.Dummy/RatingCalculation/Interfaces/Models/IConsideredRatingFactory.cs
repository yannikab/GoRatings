namespace GoRatings.Services.RatingCalculation.Interfaces;

public interface IConsideredRatingFactory
{
    IConsideredRating CreateConsideredRating(
        decimal rating,
        DateTime createdDT,
        bool isAnonymous
    );
}
