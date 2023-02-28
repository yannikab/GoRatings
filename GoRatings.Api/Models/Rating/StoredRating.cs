using GoRatings.Api.Interfaces.Rating;

namespace GoRatings.Api.Models.Rating;

public class StoredRating : GivenRating, IStoredRating
{
    public DateTime CreatedDt { get; set; }
}
