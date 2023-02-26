using System.Text.Json.Serialization;

using GoRatings.Services.RatingPersister.Interfaces;

namespace GoRatings.Services.RatingPersister.Models;

public class GivenRating : IGivenRating
{
    public Guid EntityUid { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EntityType EntityType { get; set; }

    public Guid? RaterUid { get; set; }

    public decimal Rating { get; set; }
}
