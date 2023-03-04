using GoRatings.DataAccess.UnitOfWork;
using GoRatings.Services.RatingsCleanup.Interfaces;

namespace GoRatings.Services.RatingsCleanup;

public class RatingsCleanupService : IRatingsCleanupService
{
    public int CleanUpOlderThan(DateTime cutoffDT)
    {
        using var uow = new GoRatingsUnitOfWork();

        var oldRatings = uow.Ratings.Find(r => r.CreatedDt < cutoffDT).ToList();

        uow.Ratings.RemoveRange(oldRatings);

        //uow.Complete();

        return oldRatings.Count;
    }
}
