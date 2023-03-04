using GoRatings.Services.RatingCalculation.Exceptions;
using GoRatings.Services.RatingCalculation.Interfaces;
using GoRatings.Services.RatingCalculation.Models;

namespace GoRatings.Services.Tests
{
    [TestClass]
    public class ConsideredRatingTests
    {
        public ConsideredRatingTests()
        {
        }

        [TestMethod]
        public void ValidNewRating()
        {
            IConsideredRating consideredRating = new ConsideredRating()
            {
                Rating = 5.0m,
                CreatedDT = DateTime.UtcNow,
                IsAnonymous = false,
            };

            Assert.IsTrue(consideredRating.IsValid());
        }

        [TestMethod]
        public void InvalidRatingValue()
        {
            IConsideredRating consideredRating = new ConsideredRating()
            {
                Rating = 1.3m,
                CreatedDT = DateTime.UtcNow,
                IsAnonymous = false,
            };

            Assert.IsFalse(consideredRating.IsValid());
        }

        [TestMethod]
        [ExpectedException(typeof(RatingCalculationException))]
        public void InvalidRatingFutureDate()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            IConsideredRating consideredRating = new ConsideredRating()
            {
                Rating = 5.0m,
                CreatedDT = referenceDT.AddDays(1),
                IsAnonymous = false,
            };

            decimal _ = consideredRating.EffectiveRating(referenceDT, windowDays);
        }

        [TestMethod]
        public void EffectiveRatingZeroAge()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            IConsideredRating consideredRating = new ConsideredRating()
            {
                Rating = 5.0m,
                CreatedDT = referenceDT,
                IsAnonymous = false,
            };

            Assert.IsTrue(consideredRating.IsValid());
            Assert.AreEqual(100m, consideredRating.EffectiveRating(referenceDT, windowDays));
        }

        [TestMethod]
        public void EffectiveRatingHalfAge()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            IConsideredRating consideredRating = new ConsideredRating()
            {
                Rating = 5.0m,
                CreatedDT = referenceDT.AddDays(-windowDays / 2),
                IsAnonymous = false,
            };

            Assert.IsTrue(consideredRating.IsValid());
            Assert.AreEqual(50m, consideredRating.EffectiveRating(referenceDT, windowDays));
        }

        [TestMethod]
        public void EffectiveRatingFullAge()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            IConsideredRating consideredRating = new ConsideredRating()
            {
                Rating = 5.0m,
                CreatedDT = referenceDT.AddDays(-windowDays),
                IsAnonymous = false,
            };

            Assert.IsTrue(consideredRating.IsValid());
            Assert.AreEqual(0m, consideredRating.EffectiveRating(referenceDT, windowDays));
        }

        [TestMethod]
        public void EffectiveRatingOverFullAge()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            IConsideredRating consideredRating = new ConsideredRating()
            {
                Rating = 5.0m,
                CreatedDT = referenceDT.AddDays(-2 * windowDays),
                IsAnonymous = false,
            };

            Assert.IsTrue(consideredRating.IsValid());
            Assert.AreEqual(0m, consideredRating.EffectiveRating(referenceDT, windowDays));
        }

        [TestMethod]
        public void EffectiveRatingZeroAgeAnonymous()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            IConsideredRating consideredRating = new ConsideredRating()
            {
                Rating = 5.0m,
                CreatedDT = referenceDT,
                IsAnonymous = true,
            };

            Assert.IsTrue(consideredRating.IsValid());
            Assert.AreEqual(10m, consideredRating.EffectiveRating(referenceDT, windowDays));
        }

        [TestMethod]
        public void EffectiveRatingHalfAgeAnonymous()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            IConsideredRating consideredRating = new ConsideredRating()
            {
                Rating = 5.0m,
                CreatedDT = referenceDT.AddDays(-windowDays / 2),
                IsAnonymous = true,
            };

            Assert.IsTrue(consideredRating.IsValid());
            Assert.AreEqual(5m, consideredRating.EffectiveRating(referenceDT, windowDays));
        }

        [TestMethod]
        public void EffectiveRatingFullAgeAnonymous()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            IConsideredRating consideredRating = new ConsideredRating()
            {
                Rating = 5.0m,
                CreatedDT = referenceDT.AddDays(-windowDays),
                IsAnonymous = true,
            };

            Assert.IsTrue(consideredRating.IsValid());
            Assert.AreEqual(0m, consideredRating.EffectiveRating(referenceDT, windowDays));
        }

        [TestMethod]
        public void EffectiveRatingOverFullAgeAnonymous()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            IConsideredRating consideredRating = new ConsideredRating()
            {
                Rating = 5.0m,
                CreatedDT = referenceDT.AddDays(-2 * windowDays),
                IsAnonymous = true,
            };

            Assert.IsTrue(consideredRating.IsValid());
            Assert.AreEqual(0m, consideredRating.EffectiveRating(referenceDT, windowDays));
        }
    }
}
