namespace GoRatings.Services.RatingPersister.Exceptions;

public class GivenRatingValueInvalidException : Exception
{
    private readonly Guid entityUid;
    private readonly decimal invalidValue;

    public GivenRatingValueInvalidException(Guid entityUid, decimal invalidValue)
    {
        this.entityUid = entityUid;
        this.invalidValue = invalidValue;
    }

    public GivenRatingValueInvalidException(decimal invalidValue)
        : this(Guid.Empty, invalidValue)
    {
    }

    public override string Message
    {
        get
        {
            if (entityUid != Guid.Empty)
                return $"Given rating for entity with unique id {entityUid} has a value of {invalidValue} which is invalid.";

            return $"Given rating has a value of {invalidValue} which is invalid.";
        }
    }
}
