namespace GoRatings.Services.RatingPersister.Exceptions;

public class EntityDoesNotExistException : ApplicationException
{
	private readonly Guid entityUid;

	public EntityDoesNotExistException(Guid entityUid)
	{
		this.entityUid = entityUid;
	}

	public override string Message
	{
		get { return $"Entity with unique id {entityUid} does not exist."; }
	}
}
