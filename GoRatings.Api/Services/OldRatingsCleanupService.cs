using GoRatings.DataAccess.UnitOfWork;

namespace GoRatings.Api.Services.OldRatingsCleanup;

public class OldRatingsCleanupService : IHostedService, IDisposable
{
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger<OldRatingsCleanupService> log;

    private readonly Timer timer;

    public OldRatingsCleanupService(IServiceProvider serviceProvider, ILogger<OldRatingsCleanupService> log)
    {
        this.serviceProvider = serviceProvider;
        this.log = log;
        
        timer = new Timer(CleanUpOldRatings, Settings.Instance.RatingsCleanupAgeDays, Timeout.Infinite, Timeout.Infinite);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        int startupMinutes = Settings.Instance.RatingsCleanupStartupDelayMinutes;
        int intervalMinutes = Settings.Instance.RatingsCleanupIntervalMinutes;

        log.Info($"{nameof(OldRatingsCleanupService)} starting in {startupMinutes} minutes, with an interval of {intervalMinutes} minutes.");

        timer.Change(TimeSpan.FromMinutes(startupMinutes), TimeSpan.FromMinutes(intervalMinutes));

        return Task.CompletedTask;
    }

    private void CleanUpOldRatings(object? state)
    {
        int windowDays = state != null ? (int)state : int.MaxValue;

        log.Info($"Cleaning up ratings older than {windowDays} days");

        using var scope = serviceProvider.CreateScope();

        using var uow = new GoRatingsUnitOfWork();

        var oldRatings = uow.Ratings.FindOlderThanTimeWindow(DateTime.UtcNow, windowDays);

        log.Info($"Found and removing {oldRatings} old ratings.");

        uow.Ratings.RemoveRange(oldRatings);

        uow.Complete();

        log.Info($"Old ratings removed.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        log.Info($"{nameof(OldRatingsCleanupService)} stopping.");

        timer.Change(Timeout.Infinite, Timeout.Infinite);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        log.Info($"{nameof(OldRatingsCleanupService)} disposing.");

        timer.Dispose();
        GC.SuppressFinalize(this);
    }
}
