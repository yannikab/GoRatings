using GoRatings.Services.Interfaces.Rating;

namespace GoRatings.Services.Models.Rating;

public class OverallRating : IOverallRating
{
    public DateTime CalculatedDT { get; set; }
    public int ConsideredRatings { get; set; }
    public decimal Rating { get; set; }
}
