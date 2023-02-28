using System.ComponentModel.DataAnnotations;

namespace GoRatings.Api.Contracts.RealEstateAgents;

public class CreateRealEstateAgentRequest
{
    /// <summary>
    /// A unique textual code for the real estate agent
    /// </summary>
    /// <example>RealEstateAgent_83659</example>
    [Required]
    [MaxLength(30)]
    public string Code { get; set; } = null!;

    /// <summary>
    /// A detailed description for the real estate agent.
    /// </summary>
    /// <example>Specializes in commercial real estate leasing</example>
    [Required]
    [MaxLength(2500)]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Real estate agent's first name.
    /// </summary>
    /// <example>Dana</example>
    [Required]
    [MaxLength(80)]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Real estate agent's last name.
    /// </summary>
    /// <example>Collins</example>
    [Required]
    [MaxLength(80)]
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Real estate agent's email address.
    /// </summary>
    /// <example>dana.collins@kw.com</example>
    [Required]
    [MaxLength(150)]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Real estate agent's phone number.
    /// </summary>
    /// <example>555-45387</example>
    [MaxLength(18)]
    public string Phone { get; set; } = null!;

    /// <summary>
    /// Real estate agent's license number.
    /// </summary>
    /// <example>UG836512</example>
    [Required]
    public string LicenseNumber { get; set; } = null!;

    /// <summary>
    /// Real estate agent's brokerage firm, unless they work independently.
    /// </summary>
    /// <example>Keller Williams</example>
    [MaxLength(80)]
    public string? BrokerageFirm { get; set; }
}
