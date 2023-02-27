using GoRatings.DataAccess.Models;
using GoRatings.DataAccess.UnitOfWork;
using GoRatings.Services.RatingPersister.Exceptions;
using GoRatings.Services.RatingPersister.Interfaces;

namespace GoRatings.Services.RatingPersister.Service;

public class RatingPersisterService : IRatingPersisterService
{
    public IStoredRating Add(IGivenRating givenRating)
    {
        using var uow = new GoRatingsUnitOfWork();

        var entity = uow.Entities.GetByUid(givenRating.EntityUid);

        if (entity == Entity.None)
            throw new EntityDoesNotExistException(givenRating.EntityUid);

        if (entity.GetEntityType() != givenRating.EntityType)
            throw new EntityUidTypeMismatchException(givenRating.EntityUid, givenRating.EntityType);

        var rating = givenRating.ToRating(entity);

        uow.Ratings.Add(rating);

        uow.Complete();

        return rating.ToStoredRating();
    }

    public IEnumerable<IStoredRating> GetWithinPastDays(Guid entityUid, int pastDays)
    {
        using var uow = new GoRatingsUnitOfWork();

        var entity = uow.Entities.GetByUid(entityUid);

        if (entity == Entity.None)
            throw new EntityDoesNotExistException(entityUid);

        var ratings = uow.Ratings.FindWithinTimeWindow(entityUid, DateTime.UtcNow, pastDays);

        return ratings.Select(r => r.ToStoredRating()).ToList();
    }

    public async Task<IStoredRating> AddAsync(IGivenRating givenRating)
    {
        using var uow = new GoRatingsUnitOfWork();

        var entity = await uow.Entities.GetByUidAsync(givenRating.EntityUid);

        if (entity == Entity.None)
            throw new EntityDoesNotExistException(givenRating.EntityUid);

        var rating = givenRating.ToRating(entity);

        await uow.Ratings.AddAsync(rating);

        uow.Complete();

        return rating.ToStoredRating();
    }

    public async Task<IEnumerable<IStoredRating>> GetWithinPastDaysAsync(Guid entityUid, int pastDays)
    {
        using var uow = new GoRatingsUnitOfWork();

        var entity = await uow.Entities.GetByUidAsync(entityUid);

        if (entity == Entity.None)
            throw new EntityDoesNotExistException(entityUid);

        var ratings = await uow.Ratings.FindWithinTimeWindowAsync(entityUid, DateTime.UtcNow, pastDays);

        return ratings.Select(r => r.ToStoredRating());
    }
}
