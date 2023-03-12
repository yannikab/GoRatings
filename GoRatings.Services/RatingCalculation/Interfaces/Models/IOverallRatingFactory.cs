namespace GoRatings.Services.RatingCalculation.Interfaces;

public interface IOverallRatingFactory
{
    IOverallRating CreateOverallRating(
        DateTime calculatedDT,
        int consideredRatings,
        decimal rating
    );
}
