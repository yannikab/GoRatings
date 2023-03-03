using GoRatings.Services.Caching;
using GoRatings.Services.RatingCalculation.Interfaces;
using GoRatings.Services.RatingCalculation.Models;

namespace GoRatings.Services.Tests
{
    [TestClass]
    public class CachingTests
    {
        public CachingTests()
        {
        }

        [TestMethod]
        public void AddAndRetrieveBeforeExpirationTime()
        {
            ICachingService<Guid, IOverallRating>? cachingService =
                new MemoryCachingService<Guid, IOverallRating>(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30));

            IOverallRating cached = new OverallRating() { CalculatedDT = DateTime.UtcNow, ConsideredRatings = 10, Rating = 5 };

            Guid uid = Guid.NewGuid();

            cachingService.Add(uid, cached);

            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (!cachingService.TryGetValue(uid, out var retrieved))
                Assert.Fail();

            if (!object.ReferenceEquals(cached, retrieved))
                Assert.Fail();
        }


        [TestMethod]
        public void AddAndRemoveBeforeExpirationTime()
        {
            ICachingService<Guid, IOverallRating> cachingService =
                new MemoryCachingService<Guid, IOverallRating>(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30));

            IOverallRating cached = new OverallRating() { CalculatedDT = DateTime.UtcNow, ConsideredRatings = 10, Rating = 5 };

            Guid uid = Guid.NewGuid();

            cachingService.Add(uid, cached);

            Thread.Sleep(TimeSpan.FromSeconds(1));

            cachingService.Remove(uid);
        }

        [TestMethod]
        public void RemoveItemThatIsNotInCache()
        {
            ICachingService<Guid, IOverallRating> cachingService =
                new MemoryCachingService<Guid, IOverallRating>(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30));

            Guid uid = Guid.NewGuid();

            cachingService.Remove(uid);
        }

        [TestMethod]
        public void TestImmediateExpiration()
        {
            ICachingService<Guid, IOverallRating> cachingService =
                new MemoryCachingService<Guid, IOverallRating>(TimeSpan.FromMilliseconds(1), TimeSpan.FromSeconds(30));

            IOverallRating cached = new OverallRating() { CalculatedDT = DateTime.UtcNow, ConsideredRatings = 10, Rating = 5 };

            Guid uid = Guid.NewGuid();

            cachingService.Add(uid, cached);

            Thread.Sleep(1);

            if (cachingService.TryGetValue(uid, out var _))
                Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddNullKey()
        {
            ICachingService<Guid?, IOverallRating> cachingService =
                new MemoryCachingService<Guid?, IOverallRating>(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30));

            IOverallRating cached = new OverallRating() { CalculatedDT = DateTime.UtcNow, ConsideredRatings = 10, Rating = 5 };

            Guid? uid = null;

            cachingService.Add(uid, cached);
        }

        [TestMethod]
        public void TestScanFrequency()
        {
            ICachingService<Guid, IOverallRating> cachingService =
                new MemoryCachingService<Guid, IOverallRating>(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));

            IOverallRating cached = new OverallRating() { CalculatedDT = DateTime.UtcNow, ConsideredRatings = 10, Rating = 5 };

            Guid uid = Guid.NewGuid();

            cachingService.Add(uid, cached);

            if (!cachingService.TryGetValue(uid, out var _))
                Assert.Fail();

            Thread.Sleep(1000);

            if (!cachingService.TryGetValue(uid, out var _))
                Assert.Fail();

            Thread.Sleep(5000);
        }
    }
}
