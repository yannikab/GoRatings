using GoRatings.Services.PropertyPersister;
using GoRatings.Services.PropertyPersister.Interfaces;
using GoRatings.Services.RatingPersister;
using GoRatings.Services.RatingPersister.Exceptions;
using GoRatings.Services.RatingPersister.Interfaces;
using GoRatings.Services.RatingPersister.Models;

namespace GoRatings.Services.Tests
{
    [TestClass]
    public class RatingPersisterServiceTests
    {
        private readonly IGivenRatingFactory givenRatingFactory;

        private readonly IRatingPersisterService ratingPersisterService;
        private readonly IPropertyPersisterService propertyPersisterService;

        public RatingPersisterServiceTests()
        {
            givenRatingFactory = new GivenRatingFactory();

            ratingPersisterService = new RatingPersisterService();
            propertyPersisterService = new PropertyPersisterService();
        }

        [TestMethod]
        [ExpectedException(typeof(EntityDoesNotExistException))]
        public void AddRatingNonExistentEntity()
        {
            ratingPersisterService.Add(givenRatingFactory.CreateGivenRating(
                Guid.NewGuid(),
                "Property",
                Guid.NewGuid(),
                5
            ));
        }

        [TestMethod]
        public void AddRatingExistingEntity()
        {
            var storedProperty = propertyPersisterService.GetAll().FirstOrDefault();

            if (storedProperty == null)
                Assert.Fail();

            int existingRatings = ratingPersisterService.GetWithinPastDays(storedProperty.EntityUid, 1000000).Count();

            IGivenRating givenRating = givenRatingFactory.CreateGivenRating(
                storedProperty.EntityUid,
                "Property",
                Guid.NewGuid(),
                5
            );

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
        [ExpectedException(typeof(GivenRatingValueInvalidException))]
        public void AddRatingWithInvalidValue()
        {
            var storedProperty = propertyPersisterService.GetAll().FirstOrDefault();

            if (storedProperty == null)
                Assert.Fail();

            IGivenRating givenRating = givenRatingFactory.CreateGivenRating(
                storedProperty.EntityUid,
                "Property",
                Guid.NewGuid(),
                2.3m
            );

            ratingPersisterService.Add(givenRating);
        }
    }
}
