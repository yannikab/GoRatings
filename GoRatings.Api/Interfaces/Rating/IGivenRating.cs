using GoRatings.Api.Models.Entity;

namespace GoRatings.Api.Interfaces.Rating;

public interface IGivenRating
{
    Guid EntityUid { get; set; }
    EntityType EntityType { get; set; }
    Guid? RaterUid { get; set; }
    decimal Rating { get; set; }
}
