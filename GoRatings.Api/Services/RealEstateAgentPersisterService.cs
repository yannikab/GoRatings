using GoRatings.Api.Exceptions.RealEstateAgent;
using GoRatings.Api.Interfaces.RealEstateAgent;
using GoRatings.Api.Interfaces.Services;
using GoRatings.DataAccess.Models;
using GoRatings.DataAccess.UnitOfWork;

namespace GoRatings.Api.Services.RealEstateAgentPersister;

public class RealEstateAgentPersisterService : IRealEstateAgentPersisterService
{
    public IStoredRealEstateAgent Add(IGivenRealEstateAgent givenRealEstateAgent)
    {
        using var uow = new GoRatingsUnitOfWork();

        var realEstateAgent = givenRealEstateAgent.ToRealEstateAgent();

        uow.RealEstateAgents.Add(realEstateAgent);

        uow.Complete();

        return realEstateAgent.ToStoredRealEstateAgent();
    }

    public IStoredRealEstateAgent Get(Guid entityUid)
    {
        using var uow = new GoRatingsUnitOfWork();

        var entity = uow.Entities.GetByUid(entityUid);

        if (entity == Entity.None)
            throw new RealEstateAgentDoesNotExistException(entityUid);

        uow.LoadReference(entity, e => e.RealEstateAgent);

        if (entity.RealEstateAgent == null)
            throw new RealEstateAgentDoesNotExistException(entityUid);

        return entity.RealEstateAgent.ToStoredRealEstateAgent();
    }

    public IEnumerable<IStoredRealEstateAgent> GetAll()
    {
        using var uow = new GoRatingsUnitOfWork();

        var realEstateAgents = uow.RealEstateAgents.GetAll();

        return realEstateAgents.Select(rea => rea.ToStoredRealEstateAgent()).ToList();
    }

    public async Task<IStoredRealEstateAgent> AddAsync(IGivenRealEstateAgent givenRealEstateAgent)
    {
        using var uow = new GoRatingsUnitOfWork();

        var realEstateAgent = givenRealEstateAgent.ToRealEstateAgent();

        await uow.RealEstateAgents.AddAsync(realEstateAgent);

        uow.Complete();

        return realEstateAgent.ToStoredRealEstateAgent();
    }

    public async Task<IStoredRealEstateAgent> GetAsync(Guid entityUid)
    {
        using var uow = new GoRatingsUnitOfWork();

        var entity = await uow.Entities.GetByUidAsync(entityUid);

        if (entity == Entity.None)
            throw new RealEstateAgentDoesNotExistException(entityUid);

        uow.LoadReference(entity, e => e.RealEstateAgent);

        if (entity.RealEstateAgent == null)
            throw new RealEstateAgentDoesNotExistException(entityUid);

        return entity.RealEstateAgent.ToStoredRealEstateAgent();
    }

    public async Task<IEnumerable<IStoredRealEstateAgent>> GetAllAsync()
    {
        using var uow = new GoRatingsUnitOfWork();

        var realEstateAgents = await uow.RealEstateAgents.GetAllAsync();

        return realEstateAgents.Select(p => p.ToStoredRealEstateAgent());
    }
}
