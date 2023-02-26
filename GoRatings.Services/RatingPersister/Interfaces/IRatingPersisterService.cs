namespace GoRatings.Services.RatingPersister.Interfaces;

public interface IRatingPersisterService
{
	void Add(IGivenRating rating);
	IEnumerable<IStoredRating> GetWithinPastDays(Guid entityUid, int pastDays);

	Task AddAsync(IGivenRating rating);
	Task<IEnumerable<IStoredRating>> GetWithinPastDaysAsync(Guid entityUid, int pastDays);
}
