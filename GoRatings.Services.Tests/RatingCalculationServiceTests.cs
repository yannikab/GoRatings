using GoRatings.DataAccess.Models;
using GoRatings.DataAccess.Repository;
using GoRatings.Services.RatingCalculation;
using GoRatings.Services.RatingCalculation.Exceptions;
using GoRatings.Services.RatingCalculation.Models;

namespace GoRatings.Services.Tests
{
    [TestClass]
    public class RatingCalculationServiceTests
    {
        private readonly IRatingCalculationService ratingCalculationService;
        private readonly IRepositoryRating repositoryRating;

        public RatingCalculationServiceTests()
        {
            ratingCalculationService = new RatingCalculationService();
            repositoryRating = new RepositoryRating(new GoRatingsContext());
        }

        [TestMethod]
        public void CalculateAllRatingsCurrentlyInStore()
        {
            var consideredRatings = repositoryRating.GetAll().Select(r => new ConsideredRating()
            {
                Rating = r.Value,
                CreatedDT = r.CreatedDt,
                IsAnonymous = !r.Rater.HasValue,

            }).ToList();

            DateTime referenceDT = DateTime.UtcNow;

            DateTime minRatingDate = consideredRatings.Count > 0 ? consideredRatings.Min(cr => cr.CreatedDT) : referenceDT.AddDays(-1);

            int windowDays = (int)Math.Ceiling((referenceDT - minRatingDate).TotalDays);

            var overallRating = ratingCalculationService.CalculateOverallRating(consideredRatings, referenceDT, windowDays);

            Assert.IsTrue(overallRating.IsValid);

            if (consideredRatings.Count == 0)
                Assert.AreEqual(0m, overallRating.Rating);
        }

        [TestMethod]
        [ExpectedException(typeof(RatingCalculationException))]
        public void CalculateInvalidNegativeDays()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            var consideredRatings = new ConsideredRating[] {
                new ConsideredRating() { Rating = 5.0m, CreatedDT = referenceDT, IsAnonymous = false }
            };

            ratingCalculationService.CalculateOverallRating(consideredRatings, referenceDT, -windowDays);
        }

        [TestMethod]
        [ExpectedException(typeof(RatingCalculationException))]
        public void CalculateInvalidRatingValue()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            var consideredRatings = new ConsideredRating[] {
                new ConsideredRating() { Rating = 1.2m, CreatedDT = referenceDT, IsAnonymous = false }
            };

            ratingCalculationService.CalculateOverallRating(consideredRatings, referenceDT, windowDays);
        }

        [TestMethod]
        [ExpectedException(typeof(RatingCalculationException))]
        public void CalculateInvalidRatingDate()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            var consideredRatings = new ConsideredRating[] {
                new ConsideredRating() { Rating = 5.0m, CreatedDT = referenceDT.AddDays(1), IsAnonymous = false }
            };

            ratingCalculationService.CalculateOverallRating(consideredRatings, referenceDT, windowDays);
        }

        [TestMethod]
        public void CalculateAllZeroRatingValues()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            var consideredRatings = new ConsideredRating[] {
                new ConsideredRating() { Rating = 0.0m, CreatedDT = referenceDT, IsAnonymous = false },
                new ConsideredRating() { Rating = 0.0m, CreatedDT = referenceDT, IsAnonymous = false },
                new ConsideredRating() { Rating = 0.0m, CreatedDT = referenceDT, IsAnonymous = false },
                new ConsideredRating() { Rating = 0.0m, CreatedDT = referenceDT, IsAnonymous = false },
                new ConsideredRating() { Rating = 0.0m, CreatedDT = referenceDT, IsAnonymous = false }
            };

            var overallRating = ratingCalculationService.CalculateOverallRating(consideredRatings, referenceDT, windowDays);
            Assert.AreEqual(overallRating.Rating, 0m);
            Assert.AreEqual(overallRating.ConsideredRatings, 5);
        }

        [TestMethod]
        public void CalculateAllFiveRatingValues()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            var consideredRatings = new ConsideredRating[] {
                new ConsideredRating() { Rating = 5.0m, CreatedDT = referenceDT, IsAnonymous = false },
                new ConsideredRating() { Rating = 5.0m, CreatedDT = referenceDT, IsAnonymous = false },
                new ConsideredRating() { Rating = 5.0m, CreatedDT = referenceDT, IsAnonymous = false },
                new ConsideredRating() { Rating = 5.0m, CreatedDT = referenceDT, IsAnonymous = false },
                new ConsideredRating() { Rating = 5.0m, CreatedDT = referenceDT, IsAnonymous = false }
            };

            var overallRating = ratingCalculationService.CalculateOverallRating(consideredRatings, referenceDT, windowDays);
            Assert.AreEqual(overallRating.Rating, 5m);
            Assert.AreEqual(overallRating.ConsideredRatings, 5);
        }
    }
}
