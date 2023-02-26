using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GoRatings.DataAccess.Models;

namespace GoRatings.DataAccess.Repository;

public interface IRepositoryRating : IRepository<GoRatingsContext, Rating, long>
{
    IEnumerable<Rating> FindWithinTimeWindow(Guid entityUid, DateTime referenceDT, int windowDays);
    IEnumerable<Rating> FindOlderThanTimeWindow(DateTime referenceDT, int windowDays);

    Task<IEnumerable<Rating>> FindWithinTimeWindowAsync(Guid entityUid, DateTime referenceDT, int windowDays);
    Task<IEnumerable<Rating>> FindOlderThanTimeWindowAsync(DateTime referenceDT, int windowDays);
}
