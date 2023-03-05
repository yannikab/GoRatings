using GoRatings.Services.RatingPersister.Interfaces;

namespace GoRatings.Services.RatingPersister.Models;

public class GivenRatingFactory : IGivenRatingFactory
{
    public IGivenRating CreateGivenRating(Guid entityUid, string entityType, Guid? raterUid, decimal rating)
    {
        return new GivenRating()
        {
            EntityUid = entityUid,
            EntityType = (EntityType)Enum.Parse(typeof(EntityType), entityType),
            RaterUid = raterUid,
            Rating = rating
        };
    }
}
