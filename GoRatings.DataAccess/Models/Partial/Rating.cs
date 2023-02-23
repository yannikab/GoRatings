using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoRatings.DataAccess.Models;

public partial class Rating
{
    public static Rating None { get; } = new();

    public override string ToString()
    {
        return $"{GetType().Name}: {{{nameof(Id)}: {Id}, {nameof(EntityId)}: '{EntityId}', {nameof(GivenRating)}: '{GivenRating}'}}";
    }
}
