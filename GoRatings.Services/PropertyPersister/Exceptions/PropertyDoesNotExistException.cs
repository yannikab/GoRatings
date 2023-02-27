namespace GoRatings.Services.PropertyPersister.Exceptions;

public class PropertyDoesNotExistException : ApplicationException
{
	private readonly Guid entityUid;

	public PropertyDoesNotExistException(Guid entityUid)
	{
		this.entityUid = entityUid;
	}

	public override string Message
	{
		get { return $"Property with unique id {entityUid} does not exist."; }
	}
}
