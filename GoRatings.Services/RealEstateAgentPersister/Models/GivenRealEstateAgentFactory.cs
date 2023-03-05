using GoRatings.Services.RealEstateAgentPersister.Interfaces;

namespace GoRatings.Services.RealEstateAgentPersister.Models;

public class GivenRealEstateAgentFactory : IGivenRealEstateAgentFactory
{
    public IGivenRealEstateAgent CreateGivenRealEstateAgent(
        string code,
        string description,
        string firstname,
        string lastname,
        string email,
        string phone,
        string licenseNumber,
        string? brokerageFirm)
    {
        return new GivenRealEstateAgent()
        {
            Code = code,
            Description = description,
            FirstName = firstname,
            LastName = lastname,
            Email = email,
            Phone = phone,
            LicenseNumber = licenseNumber,
            BrokerageFirm = brokerageFirm
        };
    }
}
