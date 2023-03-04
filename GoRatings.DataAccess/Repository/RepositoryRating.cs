using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using GoRatings.DataAccess.Models;

using Microsoft.EntityFrameworkCore;

namespace GoRatings.DataAccess.Repository;

public class RepositoryRating : Repository<GoRatingsContext, Rating, long>, IRepositoryRating
{
    public RepositoryRating(GoRatingsContext dbc)
        : base(dbc)
    {
    }

    private IQueryable<Rating> All
    {
        get { return dbc.Set<Rating>().Include(r => r.Entity); }
    }

    private IQueryable<Rating> Active
    {
        get { return All.Where(r => r.IsActive); }
    }

    public IEnumerable<Rating> FindWithinTimeWindow(Guid entityUid, DateTime referenceDT, int windowDays)
    {
        return Active.Where(
            r => r.Entity.Uid == entityUid &&
            r.CreatedDt < referenceDT &&
            EF.Functions.DateDiffDay(r.CreatedDt, referenceDT) < windowDays
        );
    }

    public async Task<IEnumerable<Rating>> FindWithinTimeWindowAsync(Guid entityUid, DateTime referenceDT, int windowDays)
    {
        return await Active.Where(
            r => r.Entity.Uid == entityUid &&
            r.CreatedDt < referenceDT &&
            EF.Functions.DateDiffDay(r.CreatedDt, referenceDT) < windowDays
        ).ToListAsync();
    }
}
