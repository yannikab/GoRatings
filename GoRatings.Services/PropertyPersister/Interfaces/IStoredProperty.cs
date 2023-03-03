namespace GoRatings.Services.PropertyPersister.Interfaces;

public interface IStoredProperty : IGivenProperty
{
    Guid EntityUid { get; set; }
    DateTime CreatedDT { get; set; }
    bool IsActive { get; set; }
}
