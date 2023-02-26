using System;
using System.Collections.Generic;

namespace GoRatings.DataAccess.Models;

public partial class Entity
{
    public Entity()
    {
        Ratings = new HashSet<Rating>();
    }

    public long Id { get; set; }
    public long? PropertyId { get; set; }
    public long? RealEstateAgentId { get; set; }
    public Guid Uid { get; set; }
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedDt { get; set; }
    public bool IsActive { get; set; }

    public virtual Property? Property { get; set; }
    public virtual RealEstateAgent? RealEstateAgent { get; set; }
    public virtual ICollection<Rating> Ratings { get; set; }
}
