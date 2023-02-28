﻿using GoRatings.Api.Interfaces.RealEstateAgent;

namespace GoRatings.Api.Models.RealEstateAgent;

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