using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using GoRatings.DataAccess.Models;

using Microsoft.EntityFrameworkCore;

namespace GoRatings.DataAccess.Repository;

public class RepositoryRealEstateAgent : Repository<GoRatingsContext, RealEstateAgent, long>, IRepositoryRealEstateAgent
{
    public RepositoryRealEstateAgent(GoRatingsContext dbc)
        : base(dbc)
    {
    }

    private IQueryable<RealEstateAgent> All
    {
        get { return dbc.Set<RealEstateAgent>().Include(p => p.Entity); }
    }

    public override RealEstateAgent Get(long id)
    {
        return All.FirstOrDefault(p => p.Id == id) ?? RealEstateAgent.None;
    }

    public override IEnumerable<RealEstateAgent> GetAll()
    {
        return All;
    }

    public override IEnumerable<RealEstateAgent> Find(Expression<Func<RealEstateAgent, bool>> predicate)
    {
        return All.Where(predicate);
    }

    public override async Task<RealEstateAgent> GetAsync(long id)
    {
        return await All.FirstOrDefaultAsync(p => p.Id == id) ?? RealEstateAgent.None;
    }

    public override async Task<IEnumerable<RealEstateAgent>> GetAllAsync()
    {
        return await All.ToListAsync();
    }

    public override async Task<IEnumerable<RealEstateAgent>> FindAsync(Expression<Func<RealEstateAgent, bool>> predicate)
    {
        return await All.Where(predicate).ToListAsync();
    }
}
