namespace GoRatings.Services.RealEstateAgentPersister.Interfaces;

public interface IGivenRealEstateAgentFactory
{
    IGivenRealEstateAgent CreateGivenRealEstateAgent(
        string code,
        string description,
        string firstname,
        string lastname,
        string email,
        string phone,
        string licenseNumber,
        string? brokerageFirm
    );
}
