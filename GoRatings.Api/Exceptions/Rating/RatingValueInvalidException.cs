namespace GoRatings.Api.Exceptions.Rating;

public class RatingValueInvalidException : Exception
{
    private readonly Guid entityUid;
    private readonly decimal invalidValue;

    public RatingValueInvalidException(Guid entityUid, decimal invalidValue)
    {
        this.entityUid = entityUid;
        this.invalidValue = invalidValue;
    }

    public RatingValueInvalidException(decimal invalidValue)
        : this(Guid.Empty, invalidValue)
    {
    }

    public override string Message
    {
        get
        {
            if (entityUid != Guid.Empty)
                return $"Rating for entity with unique id {entityUid} has a value of {invalidValue} which is invalid.";

            return $"Rating has a value of {invalidValue} which is invalid.";
        }
    }
}
