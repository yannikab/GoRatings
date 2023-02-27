namespace GoRatings.Services.RatingPersister.Interfaces;

public interface IRatingPersisterService
{
	IStoredRating Add(IGivenRating givenRating);
	IEnumerable<IStoredRating> GetWithinPastDays(Guid entityUid, int pastDays);

	Task<IStoredRating> AddAsync(IGivenRating givenRating);
	Task<IEnumerable<IStoredRating>> GetWithinPastDaysAsync(Guid entityUid, int pastDays);
}
