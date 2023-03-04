namespace GoRatings.Services.RatingCalculation.Interfaces;

public interface IOverallRating
{
    DateTime CalculatedDT { get; set; }
    int ConsideredRatings { get; set; }
    decimal Rating { get; set; }

    bool IsValid();
}
