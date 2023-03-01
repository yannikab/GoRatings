using GoRatings.Api.Exceptions.Property;
using GoRatings.Api.Interfaces.Property;
using GoRatings.Api.Interfaces.Services.PropertyPersister;
using GoRatings.Api.Models.Property;
using GoRatings.Api.Services.PropertyPersister;

namespace GoRatings.Api.Tests
{
    [TestClass]
    public class PropertyPersisterServiceTests
    {
        private readonly IPropertyPersisterService propertyPersisterService;

        public PropertyPersisterServiceTests()
        {
            propertyPersisterService = new PropertyPersisterService();
        }

        [TestMethod]
        public void AddAndRetrieve()
        {
            int propertiesBefore = propertyPersisterService.GetAll().Count();

            IGivenProperty givenProperty = new GivenProperty()
            {
                Code = "Property_Test",
                Description = "Property persister service test",
                Address = "Smparouni Trikorfou 8",
                City = "Athens",
                State = null,
                ZipCode = null,
                SquareFootage = 72,
                YearBuilt = 1986,
                ListingPrice = 80000m,
            };

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
