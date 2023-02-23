using System;
using System.Collections.Generic;

namespace GoRatings.DataAccess.Models;

public partial class Rating
{
    public long Id { get; set; }
    public long EntityId { get; set; }
    public Guid? Rater { get; set; }
    public decimal GivenRating { get; set; }
    public DateTime CreatedDt { get; set; }
    public bool IsActive { get; set; }

    public virtual EntityBase Entity { get; set; } = null!;
}
