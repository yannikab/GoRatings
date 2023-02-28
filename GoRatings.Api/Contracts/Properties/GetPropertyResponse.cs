using System.ComponentModel.DataAnnotations;

namespace GoRatings.Api.Contracts.Properties;

public class GetPropertyResponse : CreatePropertyRequest
{
    /// <summary>
    /// The unique id for the property.
    /// </summary>
    [Required]
    public Guid EntityUid { get; set; }

    /// <summary>
    /// The date and time that the property was stored.
    /// </summary>
    [Required]
    public DateTime CreatedDT { get; set; }

    /// <summary>
    /// Indicates if overall rating calculation is enabled for the property.
    /// </summary>
    [Required]
    public bool IsActive { get; set; }
}
