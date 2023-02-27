using GoRatings.Api.Interfaces.Property;

namespace GoRatings.Api.Models.Property;

public class StoredProperty : IStoredProperty
{
    public Guid EntityUid { get; set; }
    public DateTime CreatedDT { get; set; }
    public bool IsActive { get; set; }
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
