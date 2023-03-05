namespace GoRatings.Services.PropertyPersister.Interfaces;

public interface IGivenPropertyFactory
{
    IGivenProperty CreateGivenProperty(
        string code,
        string description,
        string address,
        string city,
        string? state,
        string? zipCode,
        short squareFootage,
        int yearBuilt,
        decimal listingPrice
    );
}
