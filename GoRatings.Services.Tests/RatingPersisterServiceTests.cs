using GoRatings.Services.Enums;
using GoRatings.Services.PropertyPersister;
using GoRatings.Services.RatingPersister;
using GoRatings.Services.RatingPersister.Exceptions;
using GoRatings.Services.RatingPersister.Interfaces;
using GoRatings.Services.RatingPersister.Models;

namespace GoRatings.Services.Tests
{
    [TestClass]
    public class RatingPersisterServiceTests
    {
        private readonly IRatingPersisterService ratingPersisterService;
        private readonly IPropertyPersisterService propertyPersisterService;

        public RatingPersisterServiceTests()
        {
            ratingPersisterService = new RatingPersisterService();
            propertyPersisterService = new PropertyPersisterService();
        }

        [TestMethod]
        [ExpectedException(typeof(EntityDoesNotExistException))]
        public void AddRatingNonExistentEntity()
        {
            ratingPersisterService.Add(new GivenRating()
            {
                EntityUid = Guid.NewGuid(),
                EntityType = EntityType.Property,
                RaterUid = Guid.NewGuid(),
                Rating = 5,
            });
        }

        [TestMethod]
        public void AddRatingExistingEntity()
        {
            var storedProperty = propertyPersisterService.GetAll().FirstOrDefault();

            if (storedProperty == null)
                Assert.Fail();

            int existingRatings = ratingPersisterService.GetWithinPastDays(storedProperty.EntityUid, 1000000).Count();

            IGivenRating givenRating = new GivenRating()
            {
                EntityUid = storedProperty.EntityUid,
                EntityType = EntityType.Property,
                RaterUid = Guid.NewGuid(),
                Rating = 5,
            };

            var storedRating = ratingPersisterService.Add(givenRating);

            int newRatings = ratingPersisterService.GetWithinPastDays(storedProperty.EntityUid, 1000000).Count();

            Assert.IsNotNull(storedRating);
            Assert.AreEqual(givenRating.EntityUid, storedRating.EntityUid);
            Assert.AreEqual(givenRating.EntityType, storedRating.EntityType);
            Assert.AreEqual(givenRating.RaterUid, storedRating.RaterUid);
            Assert.AreEqual(givenRating.Rating, storedRating.Rating);
            Assert.AreEqual(existingRatings + 1, newRatings);
        }

        [TestMethod]
        [ExpectedException(typeof(RatingValueInvalidException))]
        public void AddRatingWithInvalidValue()
        {
            var storedProperty = propertyPersisterService.GetAll().FirstOrDefault();

            if (storedProperty == null)
                Assert.Fail();

            IGivenRating givenRating = new GivenRating()
            {
                EntityUid = storedProperty.EntityUid,
                EntityType = EntityType.Property,
                RaterUid = Guid.NewGuid(),
                Rating = 2.3m,
            };

            ratingPersisterService.Add(givenRating);
        }
    }
}
