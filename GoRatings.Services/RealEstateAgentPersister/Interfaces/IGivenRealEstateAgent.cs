namespace GoRatings.Services.RealEstateAgentPersister.Interfaces;

public interface IGivenRealEstateAgent
{
    string Code { get; set; }
    string Description { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    string Email { get; set; }
    string Phone { get; set; }
    string LicenseNumber { get; set; }
    string? BrokerageFirm { get; set; }
}
