using GoRatings.Services.PropertyPersister.Interfaces;

namespace GoRatings.Services.PropertyPersister.Models;

public class GivenProperty : IGivenProperty
{
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public short SquareFootage { get; set; }
    public int YearBuilt { get; set; }
    public decimal ListingPrice { get; set; }
}
