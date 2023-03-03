namespace GoRatings.Services.RatingCalculation.Exceptions;

internal class OverallRatingInvalidException : Exception
{
    private readonly decimal invalidValue;

    public OverallRatingInvalidException(decimal invalidValue)
    {
        this.invalidValue = invalidValue;
    }

    public override string Message { get { return $"Overall rating has a value of {invalidValue} which is invalid."; } }
}
