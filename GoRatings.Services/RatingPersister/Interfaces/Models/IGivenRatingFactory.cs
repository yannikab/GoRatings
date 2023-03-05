namespace GoRatings.Services.RatingPersister.Interfaces;

public interface IGivenRatingFactory
{
    IGivenRating CreateGivenRating(
        Guid entityUid,
        string entityType,
        Guid? raterUid,
        decimal rating
    );
}
