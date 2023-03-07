namespace GoRatings.Services.RatingsCleanup.Interfaces;

public interface IRatingsCleanupService
{
    int CleanUpOlderThan(DateTime cutoffDT);
}
