using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoRatings.DataAccess.Models;

public partial class RealEstateAgent
{
    public static RealEstateAgent None { get; } = new();

    public override string ToString()
    {
        return $"{GetType().Name}: {{{nameof(Id)}: {Id}, {nameof(FirstName)}: '{FirstName}', {nameof(LastName)}: '{LastName}'}}";
    }
}
