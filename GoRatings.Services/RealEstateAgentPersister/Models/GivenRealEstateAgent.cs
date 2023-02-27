using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GoRatings.Services.RealEstateAgentPersister.Interfaces;

namespace GoRatings.Services.RealEstateAgentPersister.Models;

public class GivenRealEstateAgent : IGivenRealEstateAgent
{
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string LicenseNumber { get; set; } = null!;
    public string? BrokerageFirm { get; set; }
}
