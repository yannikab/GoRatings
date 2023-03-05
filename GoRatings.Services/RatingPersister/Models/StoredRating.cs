using GoRatings.Services.RatingPersister.Interfaces;

namespace GoRatings.Services.RatingPersister.Models;

internal class StoredRating : GivenRating, IStoredRating
{
    public DateTime CreatedDt { get; set; }
}
