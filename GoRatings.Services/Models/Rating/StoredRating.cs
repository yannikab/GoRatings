using GoRatings.Services.Interfaces.Rating;
using GoRatings.Services.Models.Entity;

namespace GoRatings.Services.Models.Rating;

public class StoredRating : IStoredRating
{
    public Guid EntityUid { get; set; }
    public EntityType EntityType { get; set; }
    public Guid? RaterUid { get; set; }
    public decimal Rating { get; set; }
	public DateTime CreatedDt { get; set; }
}
