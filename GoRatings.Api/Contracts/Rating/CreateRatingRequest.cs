namespace GoRatings.Contracts;

public class CreateRatingRequest
{
    public Guid EntityUid { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public Guid? RaterUid { get; set; }
    public decimal GivenRating { get; set; }
}
