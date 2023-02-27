using GoRatings.DataAccess.UnitOfWork;

namespace GoRatings.Api.Services.OldRatingsCleanup;

public class OldRatingsCleanupService : IHostedService, IDisposable
{
    private readonly Timer timer;
    private readonly IServiceProvider serviceProvider;

    public OldRatingsCleanupService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;

        timer = new Timer(CleanUpOldRatings, Settings.Instance.RatingsCleanupAgeDays, Timeout.Infinite, Timeout.Infinite);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        timer.Change(TimeSpan.Zero, TimeSpan.FromMinutes(Settings.Instance.RatingsCleanupIntervalMinutes));

        return Task.CompletedTask;
    }

    private void CleanUpOldRatings(object? state)
    {
        int windowDays = state != null ? (int)state : int.MaxValue;

        using var scope = serviceProvider.CreateScope();

        using var uow = new GoRatingsUnitOfWork();

        var oldRatings = uow.Ratings.FindOlderThanTimeWindow(DateTime.UtcNow, windowDays);

        uow.Ratings.RemoveRange(oldRatings);

        uow.Complete();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        timer.Change(Timeout.Infinite, Timeout.Infinite);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        timer.Dispose();
        GC.SuppressFinalize(this);
    }
}
