namespace GoRatings.Services.RatingPersister.Exceptions;

public class EntityInvalidException : ApplicationException
{
	private readonly Guid entityUid;

	public EntityInvalidException(Guid entityUid)
	{
		this.entityUid = entityUid;
	}

	public override string Message
	{
		get { return $"Entity with unique id {entityUid} is invalid."; }
	}
}
