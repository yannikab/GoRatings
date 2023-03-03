﻿using GoRatings.Services.PropertyPersister.Interfaces;

namespace GoRatings.Services.PropertyPersister;

public interface IPropertyPersisterService
{
    IStoredProperty Add(IGivenProperty givenProperty);
    IStoredProperty Get(Guid entityUid);
    IEnumerable<IStoredProperty> GetAll();

    Task<IStoredProperty> AddAsync(IGivenProperty givenProperty);
    Task<IStoredProperty> GetAsync(Guid entityUid);
    Task<IEnumerable<IStoredProperty>> GetAllAsync();
}
