using GoRatings.Api.Exceptions.Rating;
using GoRatings.Api.Interfaces.Rating;
using GoRatings.Api.Models.Rating;

namespace GoRatings.Api.Tests
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

            consideredRating.Validate();
        }

        [TestMethod]
        [ExpectedException(typeof(RatingValueInvalidException))]
        public void InvalidRatingValue()
        {
            IConsideredRating consideredRating = new ConsideredRating()
            {
                Rating = 1.3m,
                CreatedDT = DateTime.UtcNow,
                IsAnonymous = false,
            };

            consideredRating.Validate();
        }

        [TestMethod]
        [ExpectedException(typeof(RatingDateInvalidException))]
        public void InvalidRatingFutureDate()
        {
            IConsideredRating consideredRating = new ConsideredRating()
            {
                Rating = 5.0m,
                CreatedDT = DateTime.UtcNow.AddDays(1),
                IsAnonymous = false,
            };

            consideredRating.Validate();
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

            consideredRating.Validate();
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

            consideredRating.Validate();
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

            consideredRating.Validate();
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

            consideredRating.Validate();
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

            consideredRating.Validate();
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

            consideredRating.Validate();
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

            consideredRating.Validate();
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

            consideredRating.Validate();
            Assert.AreEqual(0m, consideredRating.EffectiveRating(referenceDT, windowDays));
        }
    }
}
