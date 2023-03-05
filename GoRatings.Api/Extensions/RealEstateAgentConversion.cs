using GoRatings.Api.Contracts.RealEstateAgents;
using GoRatings.Services.RealEstateAgentPersister.Interfaces;

namespace GoRatings.Api;

public static partial class Extensions
{
    public static IGivenRealEstateAgent ToGivenRealEstateAgent(this CreateRealEstateAgentRequest request, IGivenRealEstateAgentFactory givenRealEstateAgentFactory)
    {
        return givenRealEstateAgentFactory.CreateGivenRealEstateAgent(
            request.Code,
            request.Description,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone,
            request.LicenseNumber,
            request.BrokerageFirm
        );
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
