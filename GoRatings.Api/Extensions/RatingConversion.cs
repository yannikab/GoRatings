using GoRatings.Api.Contracts.Ratings;
using GoRatings.Api.Exceptions.Entity;
using GoRatings.Api.Interfaces.Rating;
using GoRatings.Api.Models.Entity;
using GoRatings.Api.Models.Rating;
using GoRatings.DataAccess.Models;

namespace GoRatings.Api;

public static partial class Extensions
{
    public static IGivenRating ToGivenRating(this CreateRatingRequest request)
    {
        return new GivenRating()
        {
            EntityUid = request.EntityUid,
            EntityType = (EntityType)Enum.Parse(typeof(EntityType), request.EntityType),
            RaterUid = request.RaterUid,
            Rating = request.Rating,
        };
    }

    public static IConsideredRating ToConsideredRating(this IStoredRating storedRating)
    {
        return new ConsideredRating()
        {
            Rating = storedRating.Rating,
            CreatedDT = storedRating.CreatedDt,
            IsAnonymous = !storedRating.RaterUid.HasValue,
        };
    }

    public static OverallRatingResponse ToOverallRatingResponse(this IOverallRating overallRating, Guid entityUid)
    {
        return new OverallRatingResponse()
        {
            EntityUid = entityUid,
            CalculatedDT = overallRating.CalculatedDT,
            ConsideredRatings = overallRating.ConsideredRatings,
            Rating = overallRating.Rating,
        };
    }

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
