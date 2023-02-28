using System.ComponentModel.DataAnnotations;

namespace GoRatings.Api.Contracts.Properties;

public class CreatePropertyRequest
{
    /// <summary>
    /// A unique textual code for the property
    /// </summary>
    /// <example>Property_91496</example>
    [Required]
    [MaxLength(30)]
    public string Code { get; set; } = null!;

    /// <summary>
    /// A detailed description for the property.
    /// </summary>
    /// <example>Luxurious 5-Bedroom Villa with Stunning Ocean View</example>
    [Required]
    [MaxLength(2500)]
    public string Description { get; set; } = null!;

    /// <summary>
    /// The address (street, number, etc.) for the property.
    /// </summary>
    /// <example>138 Palm Ave</example>
    [Required]
    [MaxLength(120)]
    public string Address { get; set; } = null!;

    /// <summary>
    /// The city where the property is located.
    /// </summary>
    /// <example>Portland</example>
    [Required]
    [MaxLength(80)]
    public string City { get; set; } = null!;

    /// <summary>
    /// The state where the property is located.
    /// </summary>
    /// <example>OR</example>
    [MaxLength(80)]
    public string? State { get; set; }

    /// <summary>
    /// The property area's zipcode.
    /// </summary>
    /// <example>97204</example>
    [MaxLength(15)]
    public string? ZipCode { get; set; }

    /// <summary>
    /// The property's square footage in square metres.
    /// </summary>
    /// <example>460</example>
    [Required]
    [Range(10, int.MaxValue)]
    public short SquareFootage { get; set; }

    /// <summary>
    /// The year when property construction was completed.
    /// </summary>
    /// <example>1996</example>
    [Required]
    [Range(1800, int.MaxValue)]
    public int YearBuilt { get; set; }

    /// <summary>
    /// The current price that the property is listed for.
    /// </summary>
    /// <example>380000</example>
    [Required]
    [Range(0, double.MaxValue)]
    public decimal ListingPrice { get; set; }
}
