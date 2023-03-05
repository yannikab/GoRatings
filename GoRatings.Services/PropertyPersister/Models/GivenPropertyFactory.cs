using GoRatings.Services.PropertyPersister.Interfaces;

namespace GoRatings.Services.PropertyPersister.Models;

public class GivenPropertyFactory : IGivenPropertyFactory
{
    public IGivenProperty CreateGivenProperty(
        string code,
        string description,
        string address,
        string city,
        string? state,
        string? zipCode,
        short squareFootage,
        int yearBuilt,
        decimal listingPrice)
    {
        return new GivenProperty()
        {
            Code = code,
            Description = description,
            Address = address,
            City = city,
            State = state,
            ZipCode = zipCode,
            SquareFootage = squareFootage,
            YearBuilt = yearBuilt,
            ListingPrice = listingPrice
        };
    }
}
