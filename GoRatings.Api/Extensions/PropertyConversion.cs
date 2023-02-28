using GoRatings.Api.Contracts.Properties;
using GoRatings.Api.Interfaces.Property;
using GoRatings.Api.Models.Property;
using GoRatings.DataAccess.Models;

namespace GoRatings.Api;

public static partial class Extensions
{
    public static IGivenProperty ToGivenProperty(this CreatePropertyRequest request)
    {
        return new GivenProperty()
        {
            Code = request.Code,
            Description = request.Description,
            Address = request.Address,
            City = request.City,
            State = request.State,
            ZipCode = request.ZipCode,
            SquareFootage = request.SquareFootage,
            YearBuilt = request.YearBuilt,
            ListingPrice = request.ListingPrice,
        };
    }

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

    public static CreatePropertyResponse ToCreatePropertyResponse(this IStoredProperty storedProperty)
    {
        return new CreatePropertyResponse()
        {
            EntityUid = storedProperty.EntityUid,
            CreatedDT = storedProperty.CreatedDT,
            IsActive = storedProperty.IsActive,
            Code = storedProperty.Code,
            Description = storedProperty.Description,
            Address = storedProperty.Address,
            City = storedProperty.City,
            State = storedProperty.State,
            ZipCode = storedProperty.ZipCode,
            SquareFootage = storedProperty.SquareFootage,
            YearBuilt = storedProperty.YearBuilt,
            ListingPrice = storedProperty.ListingPrice,
        };
    }

    public static GetPropertyResponse ToGetPropertyResponse(this IStoredProperty storedProperty)
    {
        return storedProperty.ToCreatePropertyResponse();
    }
}
