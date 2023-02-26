using GoRatings.Services.RatingPersister.Interfaces;

namespace GoRatings.Services.RatingPersister.Models;

public class StoredRating : IStoredRating
{
    public Guid EntityUid { get; set; }
    public EntityType EntityType { get; set; }
    public Guid? RaterUid { get; set; }
    public decimal Rating { get; set; }
	public DateTime CreatedDt { get; set; }
}
