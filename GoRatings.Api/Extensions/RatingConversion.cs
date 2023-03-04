using GoRatings.Api.Contracts.Ratings;
using GoRatings.Services;
using GoRatings.Services.RatingCalculation.Interfaces;
using GoRatings.Services.RatingPersister.Interfaces;
using GoRatings.Services.RatingPersister.Models;

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

    public static CreateRatingResponse ToCreateRatingResponse(this IStoredRating storedRating)
    {
        return new CreateRatingResponse()
        {
            CreatedDT = storedRating.CreatedDt,
            EntityUid = storedRating.EntityUid,
            EntityType = storedRating.EntityType.ToString(),
            RaterUid = storedRating.RaterUid,
            Rating = storedRating.Rating,
        };
    }
}
