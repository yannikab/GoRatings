namespace GoRatings.Api.Interfaces.Rating;

public interface IOverallRating
{
    DateTime CalculatedDT { get; set; }
    int ConsideredRatings { get; set; }
    decimal Rating { get; set; }
}
