using GoRatings.Services.RatingPersister.Interfaces;

namespace GoRatings.Services.RatingPersister.Models;

internal class GivenRating : IGivenRating
{
    public Guid EntityUid { get; set; }
    public EntityType EntityType { get; set; }
    public Guid? RaterUid { get; set; }
    public decimal Rating { get; set; }

    public bool IsValid()
    {
        return Rating.IsValidFiveStarRating();
    }
}
