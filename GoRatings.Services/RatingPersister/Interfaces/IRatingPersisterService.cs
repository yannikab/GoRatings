namespace GoRatings.Services.RatingPersister.Interfaces;

public interface IRatingPersisterService
{
	void Add(IGivenRating givenRating);
	IEnumerable<IStoredRating> GetWithinPastDays(Guid entityUid, int pastDays);

	Task AddAsync(IGivenRating givenRating);
	Task<IEnumerable<IStoredRating>> GetWithinPastDaysAsync(Guid entityUid, int pastDays);
}
