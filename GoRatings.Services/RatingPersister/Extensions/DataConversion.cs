using GoRatings.DataAccess.Models;
using GoRatings.Services.RatingPersister.Exceptions;
using GoRatings.Services.RatingPersister.Interfaces;
using GoRatings.Services.RatingPersister.Models;

namespace GoRatings.Services.RatingPersister;

public static partial class Extensions
{
	public static Rating ToRating(this IGivenRating gr, Entity e)
	{
		return new Rating()
		{
			EntityId = e.Id,
			Rater = gr.RaterUid,
			Value = gr.Rating,
			CreatedDt = DateTime.UtcNow,
			IsActive = true,
		};
	}

	public static IStoredRating ToStoredRatingModel(this Rating r)
	{
		return new StoredRating()
		{
			EntityUid = r.Entity.Uid,
			EntityType = r.Entity.GetEntityType(),
			RaterUid = r.Rater,
			Rating = r.Value,
			CreatedDt = r.CreatedDt,
		};
	}

	public static EntityType GetEntityType(this Entity e)
	{
		if (e.PropertyId.HasValue && e.RealEstateAgentId.HasValue)
			throw new EntityInvalidException(e.Uid);

		if (e.PropertyId.HasValue)
			return EntityType.Property;

		if (e.RealEstateAgentId.HasValue)
			return EntityType.RealEstateAgent;

		throw new EntityInvalidException(e.Uid);
	}
}
