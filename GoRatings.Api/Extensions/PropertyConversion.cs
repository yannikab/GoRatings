using GoRatings.Api.Contracts.Properties;
using GoRatings.Services.PropertyPersister.Interfaces;

namespace GoRatings.Api;

public static partial class Extensions
{
    public static IGivenProperty ToGivenProperty(this CreatePropertyRequest request, IGivenPropertyFactory givenPropertyFactory)
    {
        return givenPropertyFactory.CreateGivenProperty(
            request.Code,
            request.Description,
            request.Address,
            request.City,
            request.State,
            request.ZipCode,
            request.SquareFootage,
            request.YearBuilt,
            request.ListingPrice
        );
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
