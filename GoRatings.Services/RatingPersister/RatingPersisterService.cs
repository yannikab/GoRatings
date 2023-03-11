using GoRatings.DataAccess.Models;
using GoRatings.DataAccess.UnitOfWork;
using GoRatings.Services.RatingPersister.Exceptions;
using GoRatings.Services.RatingPersister.Interfaces;

namespace GoRatings.Services.RatingPersister;

public class RatingPersisterService : IRatingPersisterService
{
    public IStoredRating Add(IGivenRating givenRating)
    {
        if (!givenRating.IsValid())
            throw new GivenRatingValueInvalidException(givenRating.EntityUid, givenRating.Rating);

        using IGoRatingsUnitOfWork uow = new GoRatingsUnitOfWork();

        var entity = uow.Entities.GetByUid(givenRating.EntityUid);

        if (entity == Entity.None)
            throw new EntityDoesNotExistException(givenRating.EntityUid);

        if (entity.GetEntityType() != givenRating.EntityType)
            throw new EntityUidTypeMismatchException(givenRating.EntityUid, givenRating.EntityType);

        var rating = givenRating.ToRating(entity.Id);

        uow.Ratings.Add(rating);

        uow.Complete();

        var storedRating = rating.ToStoredRating();

        if (!storedRating.IsValid())
            throw new StoredRatingValueInvalidException(storedRating.EntityUid, storedRating.Rating);

        return storedRating;
    }

    public IEnumerable<IStoredRating> GetWithinPastDays(Guid entityUid, int pastDays)
    {
        using IGoRatingsUnitOfWork uow = new GoRatingsUnitOfWork();

        var entity = uow.Entities.GetByUid(entityUid);

        if (entity == Entity.None)
            throw new EntityDoesNotExistException(entityUid);

        entity.GetEntityType();

        if (!entity.IsActive)
            return Enumerable.Empty<IStoredRating>();

        var ratings = uow.Ratings.FindWithinTimeWindow(entityUid, DateTime.UtcNow, pastDays);

        var storedRatings = ratings.Select(r => r.ToStoredRating()).ToList();

        foreach (var sr in storedRatings)
            if (!sr.IsValid())
                throw new StoredRatingValueInvalidException(sr.EntityUid, sr.Rating);

        return storedRatings;
    }

    public async Task<IStoredRating> AddAsync(IGivenRating givenRating)
    {
        if (!givenRating.IsValid())
            throw new GivenRatingValueInvalidException(givenRating.EntityUid, givenRating.Rating);

        using IGoRatingsUnitOfWork uow = new GoRatingsUnitOfWork();

        var entity = await uow.Entities.GetByUidAsync(givenRating.EntityUid);

        if (entity == Entity.None)
            throw new EntityDoesNotExistException(givenRating.EntityUid);

        if (entity.GetEntityType() != givenRating.EntityType)
            throw new EntityUidTypeMismatchException(givenRating.EntityUid, givenRating.EntityType);

        var rating = givenRating.ToRating(entity.Id);

        await uow.Ratings.AddAsync(rating);

        uow.Complete();

        var storedRating = rating.ToStoredRating();

        if (!storedRating.IsValid())
            throw new StoredRatingValueInvalidException(storedRating.EntityUid, storedRating.Rating);

        return storedRating;
    }

    public async Task<IEnumerable<IStoredRating>> GetWithinPastDaysAsync(Guid entityUid, int pastDays)
    {
        using IGoRatingsUnitOfWork uow = new GoRatingsUnitOfWork();

        var entity = await uow.Entities.GetByUidAsync(entityUid);

        if (entity == Entity.None)
            throw new EntityDoesNotExistException(entityUid);

        entity.GetEntityType();

        if (!entity.IsActive)
            return Enumerable.Empty<IStoredRating>();

        var ratings = await uow.Ratings.FindWithinTimeWindowAsync(entityUid, DateTime.UtcNow, pastDays);

        var storedRatings = ratings.Select(r => r.ToStoredRating()).ToList();

        foreach (var sr in storedRatings)
            if (!sr.IsValid())
                throw new StoredRatingValueInvalidException(sr.EntityUid, sr.Rating);

        return storedRatings;
    }
}
