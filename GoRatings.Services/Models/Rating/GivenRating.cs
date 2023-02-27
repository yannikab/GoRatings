using System.Text.Json.Serialization;

using GoRatings.Services.Interfaces.Rating;
using GoRatings.Services.Models.Entity;

namespace GoRatings.Services.Models.Rating;

public class GivenRating : IGivenRating
{
    public Guid EntityUid { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EntityType EntityType { get; set; }

    public Guid? RaterUid { get; set; }

    public decimal Rating { get; set; }
}
