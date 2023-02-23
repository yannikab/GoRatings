using System;
using System.Collections.Generic;

namespace GoRatings.DataAccess.Models;

public partial class RealEstateAgent
{
    public long Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string LicenseNumber { get; set; } = null!;
    public string? BrokerageFirm { get; set; }

    public virtual EntityBase? EntityBase { get; set; }
}
