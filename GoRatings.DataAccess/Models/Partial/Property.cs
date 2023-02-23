using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoRatings.DataAccess.Models;

public partial class Property
{
    public static Property None { get; } = new();

    public override string ToString()
    {
        return $"{GetType().Name}: {{{nameof(Id)}: {Id}, {nameof(Address)}: '{Address}', {nameof(ListingPrice)}: '{ListingPrice}'}}";
    }
}
