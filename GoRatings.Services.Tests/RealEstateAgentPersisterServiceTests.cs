using GoRatings.Services.RealEstateAgentPersister;
using GoRatings.Services.RealEstateAgentPersister.Exceptions;
using GoRatings.Services.RealEstateAgentPersister.Interfaces;
using GoRatings.Services.RealEstateAgentPersister.Models;

namespace GoRatings.Services.Tests
{
    [TestClass]
    public class RealEstateAgentPersisterServiceTests
    {
        private readonly IGivenRealEstateAgentFactory givenRealEstateAgentFactory;
        private readonly IRealEstateAgentPersisterService realEstateAgentPersisterService;

        public RealEstateAgentPersisterServiceTests()
        {
            givenRealEstateAgentFactory = new GivenRealEstateAgentFactory();

            realEstateAgentPersisterService = new RealEstateAgentPersisterService();
        }

        [TestMethod]
        public void AddAndRetrieve()
        {
            int realEstateAgentsBefore = realEstateAgentPersisterService.GetAll().Count();

            IGivenRealEstateAgent givenRealEstateAgent = givenRealEstateAgentFactory.CreateGivenRealEstateAgent(
                "RealEstateAgent_Test",
                "Real estate agent persister service test",
                "Jennifer",
                "Davis",
                "jennifer.davis@compass.com",
                "555-567-8901",
                "KL123456",
                "Compass"
            );

            var storedRealEstateAgentA = realEstateAgentPersisterService.Add(givenRealEstateAgent);

            Assert.AreEqual(realEstateAgentsBefore + 1, realEstateAgentPersisterService.GetAll().Count());

            Assert.IsTrue(realEstateAgentPersisterService.GetAll().Select(sp => sp.EntityUid).Contains(storedRealEstateAgentA.EntityUid));

            var storedRealEstateAgentB = realEstateAgentPersisterService.Get(storedRealEstateAgentA.EntityUid);

            Assert.IsTrue(AreEqual(storedRealEstateAgentA, storedRealEstateAgentB));
        }

        [TestMethod]
        [ExpectedException(typeof(RealEstateAgentDoesNotExistException))]
        public void GetNonExistant()
        {
            realEstateAgentPersisterService.Get(Guid.NewGuid());
        }

        private static bool AreEqual(IStoredRealEstateAgent a, IStoredRealEstateAgent b)
        {
            return
                  a.EntityUid == b.EntityUid &&
                  a.CreatedDT - b.CreatedDT < TimeSpan.FromMilliseconds(3) &&
                  a.IsActive == b.IsActive &&
                  a.Code == b.Code &&
                  a.Description == b.Description &&
                  a.FirstName == b.FirstName &&
                  a.LastName == b.LastName &&
                  a.Email == b.Email &&
                  a.Phone == b.Phone &&
                  a.LicenseNumber == b.LicenseNumber &&
                  a.BrokerageFirm == b.BrokerageFirm;
        }
    }
}
