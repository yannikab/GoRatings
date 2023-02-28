using GoRatings.Api.Interfaces.Property;

namespace GoRatings.Api.Models.Property;

public class StoredProperty : GivenProperty, IStoredProperty
{
    public Guid EntityUid { get; set; }
    public DateTime CreatedDT { get; set; }
    public bool IsActive { get; set; }
}
