namespace GoRatings.Services.RealEstateAgentPersister.Exceptions;

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
