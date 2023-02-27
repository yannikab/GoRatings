namespace GoRatings.Api.Contracts.RealEstateAgents;

public class GetRealEstateAgentResponse : CreateRealEstateAgentRequest
{
    public Guid EntityUid { get; set; }
    public DateTime CreatedDT { get; set; }
    public bool IsActive { get; set; }
}
