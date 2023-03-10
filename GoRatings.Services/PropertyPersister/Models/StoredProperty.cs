using GoRatings.Services.PropertyPersister.Interfaces;

namespace GoRatings.Services.PropertyPersister.Models;

internal class StoredProperty : GivenProperty, IStoredProperty
{
    public Guid EntityUid { get; set; }
    public DateTime CreatedDT { get; set; }
    public bool IsActive { get; set; }
}
