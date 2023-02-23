using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoRatings.DataAccess.Models;

public partial class EntityBase
{
    public static EntityBase None { get; } = new();

    public override string ToString()
    {
        return $"{GetType().Name}: {{{nameof(Id)}: {Id}, {nameof(Uid)}: '{Uid}', {nameof(Code)}: '{Code}'}}";
    }
}
