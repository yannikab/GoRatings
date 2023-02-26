using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GoRatings.DataAccess.Models;

namespace GoRatings.DataAccess.Repository;

public interface IRepositoryEntity : IRepository<GoRatingsContext, Entity, long>
{
	Entity GetByUid(Guid entityUid);
	Task<Entity> GetByUidAsync(Guid entityUid);
}
