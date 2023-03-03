using GoRatings.DataAccess.Models;
using GoRatings.Services.RealEstateAgentPersister.Interfaces;
using GoRatings.Services.RealEstateAgentPersister.Models;

namespace GoRatings.Services;

public static partial class Extensions
{
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
}
