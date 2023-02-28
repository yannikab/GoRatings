using GoRatings.Api.Interfaces.RealEstateAgent;

namespace GoRatings.Api.Models.RealEstateAgent;

public class StoredRealEstateAgent : GivenRealEstateAgent, IStoredRealEstateAgent
{
    public Guid EntityUid { get; set; }
    public DateTime CreatedDT { get; set; }
    public bool IsActive { get; set; }
}
