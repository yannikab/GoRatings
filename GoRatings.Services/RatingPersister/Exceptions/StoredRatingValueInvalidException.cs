namespace GoRatings.Services.RatingPersister.Exceptions;

public class StoredRatingValueInvalidException : Exception
{
    private readonly Guid entityUid;
    private readonly decimal invalidValue;

    public StoredRatingValueInvalidException(Guid entityUid, decimal invalidValue)
    {
        this.entityUid = entityUid;
        this.invalidValue = invalidValue;
    }

    public StoredRatingValueInvalidException(decimal invalidValue)
        : this(Guid.Empty, invalidValue)
    {
    }

    public override string Message
    {
        get
        {
            if (entityUid != Guid.Empty)
                return $"Stored rating for entity with unique id {entityUid} has a value of {invalidValue} which is invalid.";

            return $"Stored rating has a value of {invalidValue} which is invalid.";
        }
    }
}
