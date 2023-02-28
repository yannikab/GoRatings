using GoRatings.Api.Exceptions.Entity;
using GoRatings.Api.Exceptions.Rating;
using GoRatings.Api.Interfaces.Rating;
using GoRatings.Api.Interfaces.Services.RatingPersister;
using GoRatings.DataAccess.Models;
using GoRatings.DataAccess.UnitOfWork;

namespace GoRatings.Api.Services.RatingPersister;

public class RatingPersisterService : IRatingPersisterService
{
    public IStoredRating Add(IGivenRating givenRating)
    {
        if (!givenRating.IsValid())
            throw new RatingValueInvalidException(givenRating.EntityUid, givenRating.Rating);

        using var uow = new GoRatingsUnitOfWork();

        var entity = uow.Entities.GetByUid(givenRating.EntityUid);

        if (entity == Entity.None)
            throw new EntityDoesNotExistException(givenRating.EntityUid);

        if (entity.GetEntityType() != givenRating.EntityType)
            throw new EntityUidTypeMismatchException(givenRating.EntityUid, givenRating.EntityType);

        var rating = givenRating.ToRating(entity);

        uow.Ratings.Add(rating);

        uow.Complete();

        var storedRating = rating.ToStoredRating();

        if (!storedRating.IsValid())
            throw new RatingValueInvalidException(storedRating.EntityUid, storedRating.Rating);

        return storedRating;
    }

    public IEnumerable<IStoredRating> GetWithinPastDays(Guid entityUid, int pastDays)
    {
        using var uow = new GoRatingsUnitOfWork();

        var entity = uow.Entities.GetByUid(entityUid);

        if (entity == Entity.None)
            throw new EntityDoesNotExistException(entityUid);

        if (!entity.IsActive)
            return Enumerable.Empty<IStoredRating>();

        var ratings = uow.Ratings.FindWithinTimeWindow(entityUid, DateTime.UtcNow, pastDays);

        var storedRatings = ratings.Select(r => r.ToStoredRating()).ToList();

        foreach (var sr in storedRatings)
            if (!sr.IsValid())
                throw new RatingValueInvalidException(sr.EntityUid, sr.Rating);

        return storedRatings;
    }

    public async Task<IStoredRating> AddAsync(IGivenRating givenRating)
    {
        if (!givenRating.IsValid())
            throw new RatingValueInvalidException(givenRating.EntityUid, givenRating.Rating);

        using var uow = new GoRatingsUnitOfWork();

        var entity = await uow.Entities.GetByUidAsync(givenRating.EntityUid);

        if (entity == Entity.None)
            throw new EntityDoesNotExistException(givenRating.EntityUid);

        var rating = givenRating.ToRating(entity);

        await uow.Ratings.AddAsync(rating);

        uow.Complete();

        var storedRating = rating.ToStoredRating();

        if (!storedRating.IsValid())
            throw new RatingValueInvalidException(storedRating.EntityUid, storedRating.Rating);

        return storedRating;
    }

    public async Task<IEnumerable<IStoredRating>> GetWithinPastDaysAsync(Guid entityUid, int pastDays)
    {
        using var uow = new GoRatingsUnitOfWork();

        var entity = await uow.Entities.GetByUidAsync(entityUid);

        if (entity == Entity.None)
            throw new EntityDoesNotExistException(entityUid);

        if (!entity.IsActive)
            return Enumerable.Empty<IStoredRating>();

        var ratings = await uow.Ratings.FindWithinTimeWindowAsync(entityUid, DateTime.UtcNow, pastDays);

        var storedRatings = ratings.Select(r => r.ToStoredRating()).ToList();

        foreach (var sr in storedRatings)
            if (!sr.IsValid())
                throw new RatingValueInvalidException(sr.EntityUid, sr.Rating);

        return storedRatings;
    }
}
