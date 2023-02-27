using GoRatings.Api.Contracts.Properties;
using GoRatings.Services.PropertyPersister.Interfaces;
using GoRatings.Services.PropertyPersister.Models;

namespace GoRatings.Api;

public static partial class Extensions
{
    public static GivenProperty ToGivenProperty(this CreatePropertyRequest request)
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

    public static GetPropertyResponse ToPropertyResponse(this IStoredProperty storedProperty)
    {
        return new GetPropertyResponse()
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
}
