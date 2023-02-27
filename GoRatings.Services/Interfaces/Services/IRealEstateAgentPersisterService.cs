using GoRatings.Services.Interfaces.RealEstateAgent;

namespace GoRatings.Services.Interfaces.Services;

public interface IRealEstateAgentPersisterService
{
	IStoredRealEstateAgent Add(IGivenRealEstateAgent givenRealEstateAgent);
	IStoredRealEstateAgent Get(Guid entityUid);
	IEnumerable<IStoredRealEstateAgent> GetAll();

	Task<IStoredRealEstateAgent> AddAsync(IGivenRealEstateAgent givenRealEstateAgent);
	Task<IStoredRealEstateAgent> GetAsync(Guid entityUid);
	Task<IEnumerable<IStoredRealEstateAgent>> GetAllAsync();
}
