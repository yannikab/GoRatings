namespace GoRatings.Services.Interfaces.Property;

public interface IGivenProperty
{
    string Code { get; set; }
    string Description { get; set; }
    string Address { get; set; }
    string City { get; set; }
    string? State { get; set; }
    string? ZipCode { get; set; }
    short SquareFootage { get; set; }
    int YearBuilt { get; set; }
    decimal ListingPrice { get; set; }
}
