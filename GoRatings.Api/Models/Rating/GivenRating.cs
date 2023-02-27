using System.Text.Json.Serialization;

using GoRatings.Api.Interfaces.Rating;
using GoRatings.Api.Models.Entity;

namespace GoRatings.Api.Models.Rating;

public class GivenRating : IGivenRating
{
    public Guid EntityUid { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EntityType EntityType { get; set; }

    public Guid? RaterUid { get; set; }

    public decimal Rating { get; set; }
}
