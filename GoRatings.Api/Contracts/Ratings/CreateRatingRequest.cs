using System.ComponentModel.DataAnnotations;

using GoRatings.Api.Contracts.Validation;
using GoRatings.Api.Models.Entity;

namespace GoRatings.Api.Contracts.Ratings;

public class CreateRatingRequest
{
    /// <summary>
    /// The unique id of the entity for which a rating is added.
    /// </summary>
    /// <example>0a3f3a3f-6abe-4d4f-a19b-6cafc4a4de22</example>
    [Required]
    public Guid EntityUid { get; set; }

    /// <summary>
    /// The type of the entity for which a rating is added. Valid values are "Property" and "RealEstateAgent".
    /// </summary>
    /// <example>Property</example>
    [Required]
    [ValidEnumType(typeof(EntityType))]
    public string EntityType { get; set; } = null!;

    /// <summary>
    /// The rater entity's unique id, unless the rating is anonymous.
    /// </summary>
    /// <example>87000d88-466a-4112-af2a-3db4a0d8a9a5</example>
    public Guid? RaterUid { get; set; }

    /// <summary>
    /// The rating's value.
    /// </summary>
    /// <example>4.5</example>
    [Required]
    [FiveStarRating]
    [RegularExpression(".{1,3}", ErrorMessage = $"The textual representation of {nameof(Rating)} can not be longer than 3 characters.")]
    public decimal Rating { get; set; }
}
