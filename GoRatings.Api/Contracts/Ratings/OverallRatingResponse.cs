namespace GoRatings.Api.Contracts.Ratings;

public class OverallRatingResponse
{
    public Guid EntityUid { get; set; }
    public DateTime CalculatedDT { get; set; }
    public int ConsideredRatings { get; set; }
    public decimal Rating { get; set; }
}
