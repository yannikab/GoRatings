namespace GoRatings.Api.Interfaces.Rating;

public interface IConsideredRating
{
    decimal Rating { get; set; }
    DateTime CreatedDT { get; set; }
    bool IsAnonymous { get; set; }
    void Validate();
    decimal EffectiveRating(DateTime referenceDT, int windowDays);
}
