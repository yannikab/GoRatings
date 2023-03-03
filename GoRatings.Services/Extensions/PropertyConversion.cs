using GoRatings.DataAccess.Models;
using GoRatings.Services.PropertyPersister.Interfaces;
using GoRatings.Services.PropertyPersister.Models;

namespace GoRatings.Services;

public static partial class Extensions
{
    public static Property ToProperty(this IGivenProperty givenProperty)
    {
        return new Property()
        {
            Address = givenProperty.Address,
            City = givenProperty.City,
            State = givenProperty.State,
            ZipCode = givenProperty.ZipCode,
            SquareFootage = givenProperty.SquareFootage,
            YearBuilt = givenProperty.YearBuilt,
            ListingPrice = givenProperty.ListingPrice,
            Entity = new Entity()
            {
                Uid = Guid.NewGuid(),
                Code = givenProperty.Code,
                Description = givenProperty.Description,
                CreatedDt = DateTime.UtcNow,
                IsActive = true,
            },
        };
    }

    public static IStoredProperty ToStoredProperty(this Property property)
    {
        Entity entity = property.Entity!;

        return new StoredProperty()
        {
            EntityUid = entity.Uid,
            CreatedDT = entity.CreatedDt,
            IsActive = entity.IsActive,
            Code = entity.Code,
            Description = entity.Description,
            Address = property.Address,
            City = property.City,
            State = property.State,
            ZipCode = property.ZipCode,
            SquareFootage = property.SquareFootage,
            YearBuilt = property.YearBuilt,
            ListingPrice = property.ListingPrice,
        };
    }
}
