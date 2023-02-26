using GoRatings.DataAccess.Models;
using GoRatings.DataAccess.UnitOfWork;
using GoRatings.Services.RatingPersister.Exceptions;
using GoRatings.Services.RatingPersister.Interfaces;

namespace GoRatings.Services.RatingPersister.Service;

public class RatingPersister : IRatingPersisterService
{
    public void Add(IGivenRating gr)
    {
        using var uow = new GoRatingsUnitOfWork();

        var e = uow.Entities.GetByUid(gr.EntityUid);

        if (e == Entity.None)
            throw new EntityDoesNotExistException(gr.EntityUid);

        if (e.GetEntityType() != gr.EntityType)
            throw new EntityUidTypeMismatchException(gr.EntityUid, gr.EntityType);

        uow.Ratings.Add(gr.ToRating(e));

        uow.Complete();
    }

    public IEnumerable<IStoredRating> GetWithinPastDays(Guid entityUid, int pastDays)
    {
        using var uow = new GoRatingsUnitOfWork();

        var e = uow.Entities.GetByUid(entityUid);

        if (e == Entity.None)
            throw new EntityDoesNotExistException(entityUid);

        var ratings = uow.Ratings.FindWithinPastDays(entityUid, pastDays);

        return ratings.Select(r => r.ToStoredRatingModel()).ToList();
    }

    public Task AddAsync(IGivenRating rating)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<IStoredRating>> GetWithinPastDaysAsync(Guid entityUid, int pastDays)
    {
        throw new NotImplementedException();
    }
}
