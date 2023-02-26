using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GoRatings.DataAccess.Models;

namespace GoRatings.DataAccess.Repository;

public interface IRepositoryRating : IRepository<GoRatingsContext, Rating, long>
{
	IEnumerable<Rating> FindWithinPastDays(Guid entityUid, int pastDays);
	Task<IEnumerable<Rating>> FindWithinPastDaysAsync(Guid entityUid, int pastDays);
}
