namespace GoRatings.Api.Interfaces.Rating;

public interface IStoredRating : IGivenRating
{
    public DateTime CreatedDt { get; set; }
}
