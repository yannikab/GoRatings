using GoRatings.DataAccess.Models;
using GoRatings.DataAccess.Repository;
using GoRatings.Services.RatingCalculation;
using GoRatings.Services.RatingCalculation.Exceptions;
using GoRatings.Services.RatingCalculation.Interfaces;
using GoRatings.Services.RatingCalculation.Models;

namespace GoRatings.Services.Tests
{
    [TestClass]
    public class RatingCalculationServiceTests
    {
        private readonly IConsideredRatingFactory consideredRatingFactory;

        private readonly IRatingCalculationService ratingCalculationService;
        private readonly IRepositoryRating repositoryRating;

        public RatingCalculationServiceTests()
        {
            consideredRatingFactory = new ConsideredRatingFactory();

            ratingCalculationService = new RatingCalculationService();
            repositoryRating = new RepositoryRating(new GoRatingsContext());
        }

        [TestMethod]
        public void CalculateAllRatingsCurrentlyInStore()
        {
            var consideredRatings = repositoryRating.GetAll().Select(r => consideredRatingFactory.CreateConsideredRating(
                r.Value,
                r.CreatedDt,
                !r.Rater.HasValue
            )).ToList();

            DateTime referenceDT = DateTime.UtcNow;

            DateTime minRatingDate = consideredRatings.Count > 0 ? consideredRatings.Min(cr => cr.CreatedDT) : referenceDT.AddDays(-1);

            int windowDays = (int)Math.Ceiling((referenceDT - minRatingDate).TotalDays);

            var overallRating = ratingCalculationService.CalculateOverallRating(consideredRatings, referenceDT, windowDays);

            Assert.IsTrue(overallRating.IsValid());

            if (consideredRatings.Count == 0)
                Assert.AreEqual(0m, overallRating.Rating);
        }

        [TestMethod]
        [ExpectedException(typeof(RatingCalculationException))]
        public void CalculateInvalidNegativeDays()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            var consideredRatings = new IConsideredRating[] {
                 consideredRatingFactory.CreateConsideredRating(5.0m, referenceDT, false)
            };

            ratingCalculationService.CalculateOverallRating(consideredRatings, referenceDT, -windowDays);
        }

        [TestMethod]
        [ExpectedException(typeof(RatingCalculationException))]
        public void CalculateInvalidRatingValue()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            var consideredRatings = new IConsideredRating[] {
                consideredRatingFactory.CreateConsideredRating(1.2m, referenceDT, false)
            };

            ratingCalculationService.CalculateOverallRating(consideredRatings, referenceDT, windowDays);
        }

        [TestMethod]
        [ExpectedException(typeof(RatingCalculationException))]
        public void CalculateInvalidRatingDate()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            var consideredRatings = new IConsideredRating[] {
                consideredRatingFactory.CreateConsideredRating(5.0m, referenceDT.AddDays(1), false)
            };

            ratingCalculationService.CalculateOverallRating(consideredRatings, referenceDT, windowDays);
        }

        [TestMethod]
        public void CalculateAllZeroRatingValues()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            var consideredRatings = new IConsideredRating[] {
                consideredRatingFactory.CreateConsideredRating(0.0m, referenceDT, false),
                consideredRatingFactory.CreateConsideredRating(0.0m, referenceDT, false),
                consideredRatingFactory.CreateConsideredRating(0.0m, referenceDT, false),
                consideredRatingFactory.CreateConsideredRating(0.0m, referenceDT, false),
                consideredRatingFactory.CreateConsideredRating(0.0m, referenceDT, false),
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

            var consideredRatings = new IConsideredRating[] {
                consideredRatingFactory.CreateConsideredRating(5.0m, referenceDT, false),
                consideredRatingFactory.CreateConsideredRating(5.0m, referenceDT, false),
                consideredRatingFactory.CreateConsideredRating(5.0m, referenceDT, false),
                consideredRatingFactory.CreateConsideredRating(5.0m, referenceDT, false),
                consideredRatingFactory.CreateConsideredRating(5.0m, referenceDT, false),
            };

            var overallRating = ratingCalculationService.CalculateOverallRating(consideredRatings, referenceDT, windowDays);
            Assert.AreEqual(overallRating.Rating, 5m);
            Assert.AreEqual(overallRating.ConsideredRatings, 5);
        }
    }
}
