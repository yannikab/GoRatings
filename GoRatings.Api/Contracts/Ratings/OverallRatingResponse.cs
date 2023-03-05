namespace GoRatings.Api.Contracts.Ratings;

public class OverallRatingResponse
{
    /// <summary>
    /// The unique id of the entity for which the overall rating was calculated.
    /// </summary>
    public Guid EntityUid { get; set; }

    /// <summary>
    /// The date and time when the overall rating was calculated.
    /// </summary>
    public DateTime CalculatedDT { get; set; }

    /// <summary>
    /// The number of past ratings that were considered in the overall rating calculation.
    /// </summary>
    public int ConsideredRatings { get; set; }

    /// <summary>
    /// The overall rating's value, with a precision of two decimal digits.
    /// </summary>
    public decimal Rating { get; set; }
}
