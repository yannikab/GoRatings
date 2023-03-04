using GoRatings.Api.Contracts.RealEstateAgents;
using GoRatings.Services.RealEstateAgentPersister.Interfaces;
using GoRatings.Services.RealEstateAgentPersister.Models;

namespace GoRatings.Api;

public static partial class Extensions
{
    public static IGivenRealEstateAgent ToGivenRealEstateAgent(this CreateRealEstateAgentRequest request)
    {
        return new GivenRealEstateAgent()
        {
            Code = request.Code,
            Description = request.Description,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            LicenseNumber = request.LicenseNumber,
            BrokerageFirm = request.BrokerageFirm,
        };
    }

    public static CreateRealEstateAgentResponse ToCreateRealEstateAgentResponse(this IStoredRealEstateAgent storedRealEstateAgent)
    {
        return new CreateRealEstateAgentResponse()
        {
            EntityUid = storedRealEstateAgent.EntityUid,
            CreatedDT = storedRealEstateAgent.CreatedDT,
            IsActive = storedRealEstateAgent.IsActive,
            Code = storedRealEstateAgent.Code,
            Description = storedRealEstateAgent.Description,
            FirstName = storedRealEstateAgent.FirstName,
            LastName = storedRealEstateAgent.LastName,
            Email = storedRealEstateAgent.Email,
            Phone = storedRealEstateAgent.Phone,
            LicenseNumber = storedRealEstateAgent.LicenseNumber,
            BrokerageFirm = storedRealEstateAgent.BrokerageFirm,
        };
    }

    public static GetRealEstateAgentResponse ToGetRealEstateAgentResponse(this IStoredRealEstateAgent storedRealEstateAgent)
    {
        return storedRealEstateAgent.ToCreateRealEstateAgentResponse();
    }
}
