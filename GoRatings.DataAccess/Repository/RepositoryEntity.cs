using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using GoRatings.DataAccess.Models;

namespace GoRatings.DataAccess.Repository;

public class RepositoryEntity : Repository<GoRatingsContext, Entity, long>, IRepositoryEntity
{
    public RepositoryEntity(GoRatingsContext dbc)
        : base(dbc)
    {
    }

	public Entity GetByUid(Guid entityUid)
	{
		return Find(eb => eb.Uid == entityUid).FirstOrDefault() ?? Entity.None;
	}

	public async Task<Entity> GetByUidAsync(Guid entityUid)
	{
		return (await FindAsync(eb => eb.Uid == entityUid)).FirstOrDefault() ?? Entity.None;
	}
}
