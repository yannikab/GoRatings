namespace GoRatings.Contracts;

public class RatingResponse
{
    public Guid EntityUid { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public DateTime CalculatedDT { get; set; }
    public int ConsideredRatings { get; set; }
    public decimal OverallRating { get; set; }
}
