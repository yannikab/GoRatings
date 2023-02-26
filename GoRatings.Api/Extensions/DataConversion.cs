using GoRatings.Api.Contracts.Ratings;
using GoRatings.Services.RatingCalculation.Interfaces;
using GoRatings.Services.RatingCalculation.Models;
using GoRatings.Services.RatingPersister.Interfaces;
using GoRatings.Services.RatingPersister.Models;

namespace GoRatings.Api;

public static partial class Extensions
{
	public static GivenRating ToGivenRating(this CreateRatingRequest request)
	{
		return new GivenRating()
		{
			EntityUid = request.EntityUid,
			EntityType = (EntityType)Enum.Parse(typeof(EntityType), request.EntityType),
			RaterUid = request.RaterUid,
			Rating = request.Rating,
		};
	}

	public static ConsideredRating ToConsideredRating(this IStoredRating storedRating)
	{
		return new ConsideredRating()
		{
			Rating = storedRating.Rating,
			CreatedDT = storedRating.CreatedDt,
			IsAnonymous = !storedRating.RaterUid.HasValue,
		};
	}

	public static OverallRatingResponse ToOveralRatingResponse(this IOverallRating overallRating, Guid entityUid)
	{
		return new OverallRatingResponse()
		{
			EntityUid = entityUid,
			CalculatedDT = overallRating.CalculatedDT,
			ConsideredRatings = overallRating.ConsideredRatings,
			Rating = overallRating.Rating,
		};
	}
}
