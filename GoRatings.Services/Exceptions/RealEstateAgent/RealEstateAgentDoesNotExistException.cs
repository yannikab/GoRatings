namespace GoRatings.Services.Exceptions.RealEstateAgent;

public class RealEstateAgentDoesNotExistException : ApplicationException
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
