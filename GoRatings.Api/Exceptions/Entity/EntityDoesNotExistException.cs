namespace GoRatings.Api.Exceptions.Entity;

public class EntityDoesNotExistException : Exception
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
