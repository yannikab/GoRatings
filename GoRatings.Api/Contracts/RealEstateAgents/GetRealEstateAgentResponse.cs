namespace GoRatings.Api.Contracts.RealEstateAgents;

public class GetRealEstateAgentResponse : CreateRealEstateAgentRequest
{
    /// <summary>
    /// The unique id for the real estate agent.
    /// </summary>
    public Guid EntityUid { get; set; }

    /// <summary>
    /// The date and time that the real estate agent was stored.
    /// </summary>
    public DateTime CreatedDT { get; set; }

    /// <summary>
    /// Indicates if overall rating calculation is enabled for the real estate agent.
    /// </summary>
    public bool IsActive { get; set; }
}
