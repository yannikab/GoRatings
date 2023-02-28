namespace GoRatings.Api.Contracts.Ratings;

public class CreateRatingResponse : CreateRatingRequest
{
    /// <summary>
    /// The date and time that the rating was stored.
    /// </summary>
    public DateTime CreatedDT { get; set; }
}
