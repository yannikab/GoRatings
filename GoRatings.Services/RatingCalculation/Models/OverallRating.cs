using GoRatings.Services.RatingCalculation.Interfaces;

namespace GoRatings.Services.RatingCalculation.Models;

public class OverallRating : IOverallRating
{
    public DateTime CalculatedDT { get; set; }
    public int ConsideredRatings { get; set; }
    public decimal Rating { get; set; }

    public bool IsValid { get { return !(Rating < 0 || Rating > 5); } }
}
