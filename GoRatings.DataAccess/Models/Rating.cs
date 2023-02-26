using System;
using System.Collections.Generic;

namespace GoRatings.DataAccess.Models;

public partial class Rating
{
    public long Id { get; set; }
    public long EntityId { get; set; }
    public Guid? Rater { get; set; }
    public decimal Value { get; set; }
    public DateTime CreatedDt { get; set; }
    public bool IsActive { get; set; }

    public virtual Entity Entity { get; set; } = null!;
}
