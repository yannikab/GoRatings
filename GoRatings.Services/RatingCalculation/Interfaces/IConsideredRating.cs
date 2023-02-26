namespace GoRatings.Services.RatingCalculation.Interfaces;

public interface IConsideredRating
{
    decimal Rating { get; set; }
    DateTime CreatedDT { get; set; }
    bool IsAnonymous { get; set; }
    decimal CalculateRating(DateTime referenceDT, int windowDays);
}
