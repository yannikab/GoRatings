using GoRatings.Services.RatingCalculation.Exceptions;
using GoRatings.Services.RatingCalculation.Interfaces;
using GoRatings.Services.RatingCalculation.Models;

namespace GoRatings.Services.Tests
{
    [TestClass]
    public class ConsideredRatingTests
    {
        private readonly IConsideredRatingFactory consideredRatingFactory;

        public ConsideredRatingTests()
        {
            consideredRatingFactory = new ConsideredRatingFactory();
        }

        [TestMethod]
        public void ValidNewRating()
        {
            IConsideredRating consideredRating = consideredRatingFactory.CreateConsideredRating(
                5.0m,
                DateTime.UtcNow,
                false
            );

            Assert.IsTrue(consideredRating.IsValid());
        }

        [TestMethod]
        public void InvalidRatingValue()
        {
            IConsideredRating consideredRating = consideredRatingFactory.CreateConsideredRating(
                1.3m,
                DateTime.UtcNow,
                false
            );

            Assert.IsFalse(consideredRating.IsValid());
        }

        [TestMethod]
        [ExpectedException(typeof(RatingCalculationException))]
        public void InvalidRatingFutureDate()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            IConsideredRating consideredRating = consideredRatingFactory.CreateConsideredRating(
                5.0m,
                referenceDT.AddDays(1),
                false
            );

            decimal _ = consideredRating.EffectiveRating(referenceDT, windowDays);
        }

        [TestMethod]
        public void EffectiveRatingZeroAge()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            IConsideredRating consideredRating = consideredRatingFactory.CreateConsideredRating(
                5.0m,
                referenceDT,
                false
            );

            Assert.IsTrue(consideredRating.IsValid());
            Assert.AreEqual(100m, consideredRating.EffectiveRating(referenceDT, windowDays));
        }

        [TestMethod]
        public void EffectiveRatingHalfAge()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            IConsideredRating consideredRating = consideredRatingFactory.CreateConsideredRating(
                5.0m,
                referenceDT.AddDays(-windowDays / 2),
                false
            );

            Assert.IsTrue(consideredRating.IsValid());
            Assert.AreEqual(50m, consideredRating.EffectiveRating(referenceDT, windowDays));
        }

        [TestMethod]
        public void EffectiveRatingFullAge()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            IConsideredRating consideredRating = consideredRatingFactory.CreateConsideredRating(
                5.0m,
                referenceDT.AddDays(-windowDays),
                false
            );

            Assert.IsTrue(consideredRating.IsValid());
            Assert.AreEqual(0m, consideredRating.EffectiveRating(referenceDT, windowDays));
        }

        [TestMethod]
        public void EffectiveRatingOverFullAge()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            IConsideredRating consideredRating = consideredRatingFactory.CreateConsideredRating(
                5.0m,
                referenceDT.AddDays(-2 * windowDays),
                false
            );

            Assert.IsTrue(consideredRating.IsValid());
            Assert.AreEqual(0m, consideredRating.EffectiveRating(referenceDT, windowDays));
        }

        [TestMethod]
        public void EffectiveRatingZeroAgeAnonymous()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            IConsideredRating consideredRating = consideredRatingFactory.CreateConsideredRating(
                5.0m,
                referenceDT,
                true
            );

            Assert.IsTrue(consideredRating.IsValid());
            Assert.AreEqual(10m, consideredRating.EffectiveRating(referenceDT, windowDays));
        }

        [TestMethod]
        public void EffectiveRatingHalfAgeAnonymous()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            IConsideredRating consideredRating = consideredRatingFactory.CreateConsideredRating(
                5.0m,
                referenceDT.AddDays(-windowDays / 2),
                true
            );

            Assert.IsTrue(consideredRating.IsValid());
            Assert.AreEqual(5m, consideredRating.EffectiveRating(referenceDT, windowDays));
        }

        [TestMethod]
        public void EffectiveRatingFullAgeAnonymous()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            IConsideredRating consideredRating = consideredRatingFactory.CreateConsideredRating(
                5.0m,
                referenceDT.AddDays(-windowDays),
                true
            );

            Assert.IsTrue(consideredRating.IsValid());
            Assert.AreEqual(0m, consideredRating.EffectiveRating(referenceDT, windowDays));
        }

        [TestMethod]
        public void EffectiveRatingOverFullAgeAnonymous()
        {
            DateTime referenceDT = DateTime.UtcNow;
            int windowDays = 10;

            IConsideredRating consideredRating = consideredRatingFactory.CreateConsideredRating(
                5.0m,
                referenceDT.AddDays(-2 * windowDays),
                true
            );

            Assert.IsTrue(consideredRating.IsValid());
            Assert.AreEqual(0m, consideredRating.EffectiveRating(referenceDT, windowDays));
        }
    }
}
