using GoRatings.DataAccess.Models;
using GoRatings.DataAccess.UnitOfWork;
using GoRatings.Services.PropertyPersister.Exceptions;
using GoRatings.Services.PropertyPersister.Interfaces;

namespace GoRatings.Services.PropertyPersister;

public class PropertyPersisterService : IPropertyPersisterService
{
    public IStoredProperty Add(IGivenProperty givenProperty)
    {
        using IGoRatingsUnitOfWork uow = new GoRatingsUnitOfWork();

        var property = givenProperty.ToProperty();

        uow.Properties.Add(property);

        uow.Complete();

        return property.ToStoredProperty();
    }

    public IStoredProperty Get(Guid entityUid)
    {
        using IGoRatingsUnitOfWork uow = new GoRatingsUnitOfWork();

        var entity = uow.Entities.GetByUid(entityUid);

        if (entity == Entity.None)
            throw new PropertyDoesNotExistException(entityUid);

        uow.LoadReference(entity, e => e.Property);

        if (entity.Property == null)
            throw new PropertyDoesNotExistException(entityUid);

        return entity.Property.ToStoredProperty();
    }

    public IEnumerable<IStoredProperty> GetAll()
    {
        using var uow = new GoRatingsUnitOfWork();

        var properties = uow.Properties.GetAll();

        return properties.Select(p => p.ToStoredProperty()).ToList();
    }

    public async Task<IStoredProperty> AddAsync(IGivenProperty givenProperty)
    {
        using IGoRatingsUnitOfWork uow = new GoRatingsUnitOfWork();

        var property = givenProperty.ToProperty();

        await uow.Properties.AddAsync(property);

        uow.Complete();

        return property.ToStoredProperty();
    }

    public async Task<IStoredProperty> GetAsync(Guid entityUid)
    {
        using IGoRatingsUnitOfWork uow = new GoRatingsUnitOfWork();

        var entity = await uow.Entities.GetByUidAsync(entityUid);

        if (entity == Entity.None)
            throw new PropertyDoesNotExistException(entityUid);

        await uow.LoadReferenceAsync(entity, e => e.Property);

        if (entity.Property == null)
            throw new PropertyDoesNotExistException(entityUid);

        return entity.Property.ToStoredProperty();
    }

    public async Task<IEnumerable<IStoredProperty>> GetAllAsync()
    {
        using IGoRatingsUnitOfWork uow = new GoRatingsUnitOfWork();

        var properties = await uow.Properties.GetAllAsync();

        return properties.Select(p => p.ToStoredProperty());
    }
}
