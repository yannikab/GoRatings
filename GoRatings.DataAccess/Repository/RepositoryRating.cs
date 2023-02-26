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

	public IEnumerable<Rating> FindWithinPastDays(Guid entityUid, int pastDays)
	{
		DateTime utcNow = DateTime.UtcNow;

		return Active.Where(
			r => r.Entity.Uid == entityUid &&
			r.CreatedDt < utcNow &&
			EF.Functions.DateDiffDay(r.CreatedDt, utcNow) < pastDays
		);
	}

	public async Task<IEnumerable<Rating>> FindWithinPastDaysAsync(Guid entityUid, int pastDays)
	{
		DateTime utcNow = DateTime.UtcNow;

		return await Active.Where(
			r => r.Entity.Uid == entityUid &&
			r.CreatedDt < utcNow &&
			EF.Functions.DateDiffDay(r.CreatedDt, utcNow) < pastDays
		).ToListAsync();
	}
}
