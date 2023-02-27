using System.ComponentModel.DataAnnotations;

namespace GoRatings.Api.Contracts.RealEstateAgents;

public class CreateRealEstateAgentRequest
{
    [Required]
    [MaxLength(30)]
    public string Code { get; set; } = null!;

    [Required]
    [MaxLength(2500)]
    public string Description { get; set; } = null!;

    [Required]
    [MaxLength(80)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(80)]
    public string LastName { get; set; } = null!;

    [Required]
    [MaxLength(150)]
    public string Email { get; set; } = null!;

    [MaxLength(18)]
    public string Phone { get; set; } = null!;

    [Required]
    public string LicenseNumber { get; set; } = null!;
    
    [MaxLength(80)]
    public string? BrokerageFirm { get; set; }
}
