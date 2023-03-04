namespace GoRatings.Services.RatingPersister.Interfaces;

public interface IStoredRating : IGivenRating
{
    public DateTime CreatedDt { get; set; }
}
