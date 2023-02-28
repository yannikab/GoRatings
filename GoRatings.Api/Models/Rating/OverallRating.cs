using GoRatings.Api.Interfaces.Rating;

namespace GoRatings.Api.Models.Rating;

public class OverallRating : IOverallRating
{
    public DateTime CalculatedDT { get; set; }
    public int ConsideredRatings { get; set; }
    public decimal Rating { get; set; }

    public bool IsValid()
    {
        if (Rating < 0 || Rating > 5)
            return false;

        return true;
    }
}
