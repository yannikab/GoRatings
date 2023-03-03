using GoRatings.DataAccess.Models;
using GoRatings.Services.Enums;
using GoRatings.Services.RatingPersister.Exceptions;
using GoRatings.Services.RatingPersister.Interfaces;
using GoRatings.Services.RatingPersister.Models;

namespace GoRatings.Services;

public static partial class Extensions
{
    public static Rating ToRating(this IGivenRating givenRating, Entity entity)
    {
        return new Rating()
        {
            EntityId = entity.Id,
            Rater = givenRating.RaterUid,
            Value = givenRating.Rating,
            CreatedDt = DateTime.UtcNow,
            IsActive = true,
        };
    }

    public static IStoredRating ToStoredRating(this Rating rating)
    {
        return new StoredRating()
        {
            EntityUid = rating.Entity.Uid,
            EntityType = rating.Entity.GetEntityType(),
            RaterUid = rating.Rater,
            Rating = rating.Value,
            CreatedDt = rating.CreatedDt,
        };
    }

    //public static IConsideredRating ToConsideredRating(this IStoredRating storedRating)
    //{
    //    return new ConsideredRating()
    //    {
    //        Rating = storedRating.Rating,
    //        CreatedDT = storedRating.CreatedDt,
    //        IsAnonymous = !storedRating.RaterUid.HasValue,
    //    };
    //}

    public static EntityType GetEntityType(this Entity entity)
    {
        if (entity.PropertyId.HasValue && entity.RealEstateAgentId.HasValue)
            throw new EntityInvalidException(entity.Uid);

        if (entity.PropertyId.HasValue)
            return EntityType.Property;

        if (entity.RealEstateAgentId.HasValue)
            return EntityType.RealEstateAgent;

        throw new EntityInvalidException(entity.Uid);
    }
}
