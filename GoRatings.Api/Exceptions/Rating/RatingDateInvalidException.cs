namespace GoRatings.Api.Exceptions.Rating;

public class RatingDateInvalidException : Exception
{
    private readonly Guid entityUid;
    private readonly DateTime invalidDate;

    public RatingDateInvalidException(Guid entityUid, DateTime invalidDate)
    {
        this.entityUid = entityUid;
        this.invalidDate = invalidDate;
    }

    public RatingDateInvalidException(DateTime invalidDate)
        : this(Guid.Empty, invalidDate)
    {
    }

    public override string Message
    {
        get
        {
            if (entityUid != Guid.Empty)
                return $"Rating for entity with unique id {entityUid} has a date of {invalidDate} which is invalid.";

            return $"Rating has a date of {invalidDate} which is invalid.";
        }
    }
}
