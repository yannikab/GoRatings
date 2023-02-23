using System.ComponentModel.DataAnnotations;

using GoRatings.Api.Contracts.Validation;
using GoRatings.Api.Models;

namespace GoRatings.Api.Contracts.Ratings;

public class CreateRatingRequest
{
    [Required]
    public Guid EntityUid { get; set; }

    [Required]
    [ValidEnumType(typeof(EntityType))]
    public string EntityType { get; set; } = string.Empty;
    
    public Guid? RaterUid { get; set; }
    
    [Required]
    [FiveStarRating]
    [RegularExpression(".{0,4}", ErrorMessage = $"The textual representation of {nameof(GivenRating)} can not be longer than 4 characters.")]
    public decimal GivenRating { get; set; }
}
