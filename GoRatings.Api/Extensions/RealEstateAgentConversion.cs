using GoRatings.Api.Contracts.RealEstateAgents;
using GoRatings.Services.Interfaces.RealEstateAgent;
using GoRatings.Services.Models.RealEstateAgent;

namespace GoRatings.Api;

public static partial class Extensions
{
    public static GivenRealEstateAgent ToGivenRealEstateAgent(this CreateRealEstateAgentRequest request)
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

    public static GetRealEstateAgentResponse ToRealEstateAgentResponse(this IStoredRealEstateAgent storedRealEstateAgent)
    {
        return new GetRealEstateAgentResponse()
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
}
