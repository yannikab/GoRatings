using GoRatings.Api.Contracts.Ratings;
using GoRatings.Services.RatingCalculation.Interfaces;
using GoRatings.Services.RatingPersister.Interfaces;

namespace GoRatings.Api;

public static partial class Extensions
{
    public static IGivenRating ToGivenRating(this CreateRatingRequest request, IGivenRatingFactory givenRatingFactory)
    {
        return givenRatingFactory.CreateGivenRating(
            request.EntityUid,
            request.EntityType,
            request.RaterUid,
            request.Rating
        );
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

    public static IConsideredRating ToConsideredRating(this IStoredRating storedRating, IConsideredRatingFactory consideredRatingFactory)
    {
        return consideredRatingFactory.CreateConsideredRating(
            storedRating.Rating,
            storedRating.CreatedDt,
            !storedRating.RaterUid.HasValue
        );
    }
}
