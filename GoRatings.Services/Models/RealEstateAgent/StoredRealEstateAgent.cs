using GoRatings.Services.Interfaces.RealEstateAgent;

namespace GoRatings.Services.Models.RealEstateAgent;

public class StoredRealEstateAgent : IStoredRealEstateAgent
{
    public Guid EntityUid { get; set; }
    public DateTime CreatedDT { get; set; }
    public bool IsActive { get; set; }
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string LicenseNumber { get; set; } = null!;
    public string? BrokerageFirm { get; set; }
}
