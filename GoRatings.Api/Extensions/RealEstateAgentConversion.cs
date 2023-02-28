using GoRatings.Api.Contracts.RealEstateAgents;
using GoRatings.Api.Interfaces.RealEstateAgent;
using GoRatings.Api.Models.RealEstateAgent;
using GoRatings.DataAccess.Models;

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

    public static RealEstateAgent ToRealEstateAgent(this IGivenRealEstateAgent givenRealEstateAgent)
    {
        return new RealEstateAgent()
        {
            FirstName = givenRealEstateAgent.FirstName,
            LastName = givenRealEstateAgent.LastName,
            Email = givenRealEstateAgent.Email,
            Phone = givenRealEstateAgent.Phone,
            LicenseNumber = givenRealEstateAgent.LicenseNumber,
            BrokerageFirm = givenRealEstateAgent.BrokerageFirm,
            Entity = new Entity()
            {
                Uid = Guid.NewGuid(),
                Code = givenRealEstateAgent.Code,
                Description = givenRealEstateAgent.Description,
                CreatedDt = DateTime.UtcNow,
                IsActive = true,
            },
        };
    }

    public static IStoredRealEstateAgent ToStoredRealEstateAgent(this RealEstateAgent realEstateAgent)
    {
        Entity entity = realEstateAgent.Entity!;

        return new StoredRealEstateAgent()
        {
            EntityUid = entity.Uid,
            CreatedDT = entity.CreatedDt,
            IsActive = entity.IsActive,
            Code = entity.Code,
            Description = entity.Description,
            FirstName = realEstateAgent.FirstName,
            LastName = realEstateAgent.LastName,
            Email = realEstateAgent.Email,
            Phone = realEstateAgent.Phone,
            LicenseNumber = realEstateAgent.LicenseNumber,
            BrokerageFirm = realEstateAgent.BrokerageFirm,
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
