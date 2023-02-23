using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using GoRatings.DataAccess.Models;

namespace GoRatings.DataAccess.Repository;

public class RepositoryRating : Repository<GoRatingsContext, Rating, long>, IRepositoryRating
{
    public RepositoryRating(GoRatingsContext dbc)
        : base(dbc)
    {
    }
}
