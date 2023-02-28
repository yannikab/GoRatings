using GoRatings.Api.Interfaces.RealEstateAgent;

namespace GoRatings.Api.Interfaces.Services.RealEstateAgentPersister;

public interface IRealEstateAgentPersisterService
{
    IStoredRealEstateAgent Add(IGivenRealEstateAgent givenRealEstateAgent);
    IStoredRealEstateAgent Get(Guid entityUid);
    IEnumerable<IStoredRealEstateAgent> GetAll();

    Task<IStoredRealEstateAgent> AddAsync(IGivenRealEstateAgent givenRealEstateAgent);
    Task<IStoredRealEstateAgent> GetAsync(Guid entityUid);
    Task<IEnumerable<IStoredRealEstateAgent>> GetAllAsync();
}
