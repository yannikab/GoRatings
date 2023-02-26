using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using GoRatings.DataAccess.Models;

using Microsoft.EntityFrameworkCore;

namespace GoRatings.DataAccess.Repository;

public class RepositoryProperty : Repository<GoRatingsContext, Property, long>, IRepositoryProperty
{
    public RepositoryProperty(GoRatingsContext dbc)
        : base(dbc)
    {
    }

    private IQueryable<Property> All
    {
        get { return dbc.Set<Property>().Include(p => p.Entity); }
    }

    public override Property Get(long id)
    {
        return All.FirstOrDefault(p => p.Id == id) ?? Property.None;
    }

    public override IEnumerable<Property> GetAll()
    {
        return All;
    }

    public override IEnumerable<Property> Find(Expression<Func<Property, bool>> predicate)
    {
        return All.Where(predicate);
    }

    public override async Task<Property> GetAsync(long id)
    {
        return await All.FirstOrDefaultAsync(p => p.Id == id) ?? Property.None;
    }

    public override async Task<IEnumerable<Property>> GetAllAsync()
    {
        return await All.ToListAsync();
    }

    public override async Task<IEnumerable<Property>> FindAsync(Expression<Func<Property, bool>> predicate)
    {
        return await All.Where(predicate).ToListAsync();
    }
}
