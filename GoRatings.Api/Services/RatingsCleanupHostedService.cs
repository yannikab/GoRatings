using GoRatings.Services.RatingsCleanup.Interfaces;

namespace GoRatings.Api.Services.RatingsCleanup;

public class RatingsCleanupHostedService : IHostedService, IDisposable
{
    private readonly IRatingsCleanupService oldRatingsCleanupService;
    private readonly ILogger<RatingsCleanupHostedService> log;

    private readonly Timer timer;

    public RatingsCleanupHostedService(IRatingsCleanupService ratingsCleanupService, ILogger<RatingsCleanupHostedService> log)
    {
        this.oldRatingsCleanupService = ratingsCleanupService;
        this.log = log;

        timer = new Timer(CleanUpOldRatings, Settings.Instance.RatingsCleanupAgeDays, Timeout.Infinite, Timeout.Infinite);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        int startupMinutes = Settings.Instance.RatingsCleanupStartupDelayMinutes;
        int intervalMinutes = Settings.Instance.RatingsCleanupIntervalMinutes;

        log.Info($"{nameof(RatingsCleanupHostedService)} starting in {startupMinutes} minutes, with an interval of {intervalMinutes} minutes.");

        timer.Change(TimeSpan.FromMinutes(startupMinutes), TimeSpan.FromMinutes(intervalMinutes));

        return Task.CompletedTask;
    }

    private void CleanUpOldRatings(object? state)
    {
        int windowDays = state != null ? (int)state : 0;

        log.Info($"Cleaning up ratings older than {windowDays} days");

        int removed = oldRatingsCleanupService.CleanUpOlderThan(DateTime.UtcNow.AddDays(-windowDays));

        log.Info($"Removed {removed} old ratings.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        log.Info($"{nameof(RatingsCleanupHostedService)} stopping.");

        timer.Change(Timeout.Infinite, Timeout.Infinite);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        log.Info($"{nameof(RatingsCleanupHostedService)} disposing.");

        timer.Dispose();
        GC.SuppressFinalize(this);
    }
}
