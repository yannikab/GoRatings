using System;
using System.Collections.Generic;

namespace GoRatings.DataAccess.Models;

public partial class Property
{
    public long Id { get; set; }
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public short SquareFootage { get; set; }
    public int YearBuilt { get; set; }
    public decimal ListingPrice { get; set; }

    public virtual EntityBase? EntityBase { get; set; }
}
