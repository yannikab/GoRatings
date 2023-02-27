using GoRatings.Services.Models.Entity;

namespace GoRatings.Services.Interfaces.Rating;

public interface IGivenRating
{
	Guid EntityUid { get; set; }
	EntityType EntityType { get; set; }
	Guid? RaterUid { get; set; }
	decimal Rating { get; set; }
}
