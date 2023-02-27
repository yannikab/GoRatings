using System.ComponentModel.DataAnnotations;

using GoRatings.Api.Contracts.Validation;
using GoRatings.Api.Models.Entity;

namespace GoRatings.Api.Contracts.Ratings;

public class CreateRatingRequest
{
    [Required]
    public Guid EntityUid { get; set; }

    [Required]
    [ValidEnumType(typeof(EntityType))]
    public string EntityType { get; set; } = null!;

    public Guid? RaterUid { get; set; }

    [Required]
    [FiveStarRating]
    [RegularExpression(".{0,3}", ErrorMessage = $"The textual representation of {nameof(Rating)} can not be longer than 3 characters.")]
    public decimal Rating { get; set; }
}
