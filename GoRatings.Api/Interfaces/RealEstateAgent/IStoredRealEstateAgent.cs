﻿namespace GoRatings.Api.Interfaces.RealEstateAgent;

public interface IStoredRealEstateAgent : IGivenRealEstateAgent
{
    Guid EntityUid { get; set; }
    DateTime CreatedDT { get; set; }
    bool IsActive { get; set; }
}