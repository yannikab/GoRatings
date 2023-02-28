namespace GoRatings.Api.Exceptions.RealEstateAgent;

public class RealEstateAgentDoesNotExistException : Exception
{
    private readonly Guid entityUid;

    public RealEstateAgentDoesNotExistException(Guid entityUid)
    {
        this.entityUid = entityUid;
    }

    public override string Message
    {
        get { return $"Real Estate Agent with unique id {entityUid} does not exist."; }
    }
}
