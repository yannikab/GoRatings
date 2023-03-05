using GoRatings.Services.PropertyPersister;
using GoRatings.Services.PropertyPersister.Exceptions;
using GoRatings.Services.PropertyPersister.Interfaces;
using GoRatings.Services.PropertyPersister.Models;

namespace GoRatings.Services.Tests
{
    [TestClass]
    public class PropertyPersisterServiceTests
    {
        private readonly IGivenPropertyFactory givenPropertyFactory;

        private readonly IPropertyPersisterService propertyPersisterService;

        public PropertyPersisterServiceTests()
        {
            givenPropertyFactory = new GivenPropertyFactory();

            propertyPersisterService = new PropertyPersisterService();
        }

        [TestMethod]
        public void AddAndRetrieve()
        {
            int propertiesBefore = propertyPersisterService.GetAll().Count();

            IGivenProperty givenProperty = givenPropertyFactory.CreateGivenProperty(
                "Property_Test",
                "Property persister service test",
                "Smparouni Trikorfou 8",
                "Athens",
                null,
                null,
                72,
                1986,
                80000m
            );

            var storedPropertyA = propertyPersisterService.Add(givenProperty);

            Assert.AreEqual(propertiesBefore + 1, propertyPersisterService.GetAll().Count());

            Assert.IsTrue(propertyPersisterService.GetAll().Select(sp => sp.EntityUid).Contains(storedPropertyA.EntityUid));

            var storedPropertyB = propertyPersisterService.Get(storedPropertyA.EntityUid);

            Assert.IsTrue(AreEqual(storedPropertyA, storedPropertyB));
        }

        [TestMethod]
        [ExpectedException(typeof(PropertyDoesNotExistException))]
        public void GetNonExistant()
        {
            propertyPersisterService.Get(Guid.NewGuid());
        }

        private static bool AreEqual(IStoredProperty a, IStoredProperty b)
        {
            return
                  a.EntityUid == b.EntityUid &&
                  a.CreatedDT - b.CreatedDT < TimeSpan.FromMilliseconds(3) &&
                  a.IsActive == b.IsActive &&
                  a.Code == b.Code &&
                  a.Description == b.Description &&
                  a.Address == b.Address &&
                  a.City == b.City &&
                  a.State == b.State &&
                  a.ZipCode == b.ZipCode &&
                  a.SquareFootage == b.SquareFootage &&
                  a.YearBuilt == b.YearBuilt &&
                  a.ListingPrice == b.ListingPrice;
        }
    }
}
