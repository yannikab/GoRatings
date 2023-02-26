using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GoRatings.DataAccess.UnitOfWork;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GoRatings.Services.OldRatingsCleanup.Service
{
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
}
