namespace GoRatings.Api.Contracts.Properties;

public class GetPropertyResponse : CreatePropertyRequest
{
    public Guid EntityUid { get; set; }
    public DateTime CreatedDT { get; set; }
    public bool IsActive { get; set; }
}
