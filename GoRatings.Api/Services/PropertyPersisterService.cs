using GoRatings.Api.Exceptions.Property;
using GoRatings.Api.Interfaces.Property;
using GoRatings.Api.Interfaces.Services;
using GoRatings.DataAccess.Models;
using GoRatings.DataAccess.UnitOfWork;

namespace GoRatings.Api.Services.PropertyPersister;

public class PropertyPersisterService : IPropertyPersisterService
{
    public IStoredProperty Add(IGivenProperty givenProperty)
    {
        using var uow = new GoRatingsUnitOfWork();

        var property = givenProperty.ToProperty();

        uow.Properties.Add(property);

        uow.Complete();

        return property.ToStoredProperty();
    }

    public IStoredProperty Get(Guid entityUid)
    {
        using var uow = new GoRatingsUnitOfWork();

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
        using var uow = new GoRatingsUnitOfWork();

        var property = givenProperty.ToProperty();

        await uow.Properties.AddAsync(property);

        uow.Complete();

        return property.ToStoredProperty();
    }

    public async Task<IStoredProperty> GetAsync(Guid entityUid)
    {
        using var uow = new GoRatingsUnitOfWork();

        var entity = await uow.Entities.GetByUidAsync(entityUid);

        if (entity == Entity.None)
            throw new PropertyDoesNotExistException(entityUid);

        uow.LoadReference(entity, e => e.Property);

        if (entity.Property == null)
            throw new PropertyDoesNotExistException(entityUid);

        return entity.Property.ToStoredProperty();
    }

    public async Task<IEnumerable<IStoredProperty>> GetAllAsync()
    {
        using var uow = new GoRatingsUnitOfWork();

        var properties = await uow.Properties.GetAllAsync();

        return properties.Select(p => p.ToStoredProperty());
    }
}
