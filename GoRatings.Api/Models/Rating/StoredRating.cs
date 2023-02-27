using GoRatings.Api.Interfaces.Rating;
using GoRatings.Api.Models.Entity;

namespace GoRatings.Api.Models.Rating;

public class StoredRating : IStoredRating
{
    public Guid EntityUid { get; set; }
    public EntityType EntityType { get; set; }
    public Guid? RaterUid { get; set; }
    public decimal Rating { get; set; }
    public DateTime CreatedDt { get; set; }
}
