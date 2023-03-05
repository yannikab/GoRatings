using GoRatings.Services.RealEstateAgentPersister.Interfaces;

namespace GoRatings.Services.RealEstateAgentPersister.Models;

internal class StoredRealEstateAgent : GivenRealEstateAgent, IStoredRealEstateAgent
{
    public Guid EntityUid { get; set; }
    public DateTime CreatedDT { get; set; }
    public bool IsActive { get; set; }
}
