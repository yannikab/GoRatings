using System.ComponentModel.DataAnnotations;

namespace GoRatings.Api.Contracts.Properties;

public class CreatePropertyRequest
{
    [Required]
    [MaxLength(30)]
    public string Code { get; set; } = null!;

    [Required]
    [MaxLength(2500)]
    public string Description { get; set; } = null!;

    [Required]
    [MaxLength(120)]
    public string Address { get; set; } = null!;

    [Required]
    [MaxLength(80)]
    public string City { get; set; } = null!;

    [MaxLength(80)]
    public string? State { get; set; }

    [MaxLength(15)]
    public string? ZipCode { get; set; }

    [Required]
    public short SquareFootage { get; set; }
    
    [Required]
    public int YearBuilt { get; set; }
    
    [Required]
    public decimal ListingPrice { get; set; }
}
