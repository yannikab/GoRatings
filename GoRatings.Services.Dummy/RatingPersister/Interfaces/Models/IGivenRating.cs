namespace GoRatings.Services.RatingPersister.Interfaces;

public interface IGivenRating
{
    Guid EntityUid { get; set; }
    EntityType EntityType { get; set; }
    Guid? RaterUid { get; set; }
    decimal Rating { get; set; }

    bool IsValid();
}
