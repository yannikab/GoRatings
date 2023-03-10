using System.Diagnostics.CodeAnalysis;
using System.Net;

using GoRatings.Api.Contracts.Ratings;
using GoRatings.Api.Controllers;
using GoRatings.DataAccess.Models;
using GoRatings.Services;
using GoRatings.Services.Caching;
using GoRatings.Services.Caching.Interfaces;
using GoRatings.Services.RatingCalculation.Interfaces;
using GoRatings.Services.RatingCalculation.Models;
using GoRatings.Services.RatingPersister.Exceptions;
using GoRatings.Services.RatingPersister.Interfaces;
using GoRatings.Services.RatingPersister.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using Moq;

namespace GoRatings.Api.Tests;

[SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]

[TestClass]
public class RatingsControllerCreateRatingTests
{
    readonly IGivenRatingFactory givenRatingFactory = new GivenRatingFactory();
    readonly IConsideredRatingFactory consideredRatingFactory = new ConsideredRatingFactory();

    readonly Mock<IRatingPersisterService> mockRatingPersisterService = new Mock<IRatingPersisterService>();
    readonly Mock<IRatingCalculationService> mockRatingCalculationService = new Mock<IRatingCalculationService>();
    readonly ICachingService<Guid, OverallRatingResponse> cachingService = new MemoryCachingService<Guid, OverallRatingResponse>(TimeSpan.FromMinutes(60), TimeSpan.FromMinutes(10));
    readonly ILogger<RatingsController> logger = new NullLoggerFactory().CreateLogger<RatingsController>();
    readonly Mock<IHostApplicationLifetime> mockHostApplicationLifeTime = new Mock<IHostApplicationLifetime>();

    [TestMethod]
    public void ValidRatingReturnsOk()
    {
        Guid entityUid = Guid.NewGuid();
        string entityType = "Property";
        Guid raterUid = Guid.NewGuid();
        decimal givenRating = 4.5m;

        var rating = givenRatingFactory
            .CreateGivenRating(entityUid, entityType, raterUid, givenRating)
            .ToRating(12);

        rating.Entity = new Entity() { Uid = entityUid, PropertyId = 42 };

        var storedRating = rating.ToStoredRating();

        mockRatingPersisterService
            .Setup(rps => rps.AddAsync(It.IsAny<IGivenRating>()))
            .Returns(Task.FromResult(storedRating));

        RatingsController ratingsController = new RatingsController(
            givenRatingFactory,
            consideredRatingFactory,
            mockRatingPersisterService.Object,
            mockRatingCalculationService.Object,
            cachingService,
            logger,
            mockHostApplicationLifeTime.Object
        );

        var createRatingRequest = new CreateRatingRequest()
        {
            EntityUid = entityUid,
            EntityType = "Property",
            RaterUid = raterUid,
            Rating = givenRating,
        };

        var actionResult = ratingsController.CreateRating(createRatingRequest).GetAwaiter().GetResult();
        Assert.IsInstanceOfType(actionResult, typeof(CreatedAtActionResult));

        var createdAtActionResult = (CreatedAtActionResult)actionResult;
        Assert.IsNotNull(createdAtActionResult.Value);
        Assert.IsInstanceOfType(createdAtActionResult.Value, typeof(CreateRatingResponse));
        Assert.AreEqual(createdAtActionResult.ActionName, "GetRating");

        var createRatingResponse = (CreateRatingResponse)createdAtActionResult.Value;
        Assert.AreEqual(createRatingRequest.EntityUid, createRatingResponse.EntityUid);
        Assert.AreEqual(createRatingRequest.EntityType, createRatingResponse.EntityType);
        Assert.AreEqual(createRatingRequest.RaterUid, createRatingResponse.RaterUid);
        Assert.AreEqual(createRatingRequest.Rating, createRatingResponse.Rating);
    }

    [TestMethod]
    public void GivenRatingValueInvalidExceptionReturnsBadRequest()
    {
        Guid entityUid = Guid.NewGuid();
        decimal givenRating = 3.3m;

        mockRatingPersisterService
            .Setup(rps => rps.AddAsync(It.IsAny<IGivenRating>()))
            .Throws(() => new GivenRatingValueInvalidException(entityUid, givenRating));

        RatingsController ratingsController = new RatingsController(
            givenRatingFactory,
            consideredRatingFactory,
            mockRatingPersisterService.Object,
            mockRatingCalculationService.Object,
            cachingService,
            logger,
            mockHostApplicationLifeTime.Object
        );

        var createRatingRequest = new CreateRatingRequest()
        {
            EntityUid = entityUid,
            EntityType = "Property",
            RaterUid = null,
            Rating = givenRating,
        };

        var actionResult = ratingsController.CreateRating(createRatingRequest).GetAwaiter().GetResult();
        Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));

        var badRequestObjectResult = (BadRequestObjectResult)actionResult;
        Assert.IsInstanceOfType(badRequestObjectResult.Value, typeof(string));
        Assert.IsNotNull(badRequestObjectResult.Value);
        Assert.IsTrue(((string)badRequestObjectResult.Value).ToLower().Contains(entityUid.ToString().ToLower()));
    }

    [TestMethod]
    public void EntityUidTypeMismatchExceptionReturnsBadRequest()
    {
        Guid entityUid = Guid.NewGuid();
        EntityType entityType = EntityType.Property;
        decimal givenRating = 3.3m;

        mockRatingPersisterService
            .Setup(rps => rps.AddAsync(It.IsAny<IGivenRating>()))
            .Throws(() => new EntityUidTypeMismatchException(entityUid, entityType));

        RatingsController ratingsController = new RatingsController(
            givenRatingFactory,
            consideredRatingFactory,
            mockRatingPersisterService.Object,
            mockRatingCalculationService.Object,
            cachingService,
            logger,
            mockHostApplicationLifeTime.Object
        );

        var createRatingRequest = new CreateRatingRequest()
        {
            EntityUid = entityUid,
            EntityType = "Property",
            RaterUid = null,
            Rating = givenRating,
        };

        var actionResult = ratingsController.CreateRating(createRatingRequest).GetAwaiter().GetResult();
        Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));

        var badRequestObjectResult = (BadRequestObjectResult)actionResult;
        Assert.IsInstanceOfType(badRequestObjectResult.Value, typeof(string));
        Assert.IsNotNull(badRequestObjectResult.Value);
        Assert.IsTrue(((string)badRequestObjectResult.Value).ToLower().Contains(entityUid.ToString().ToLower()));
    }

    [TestMethod]
    public void EntityDoesNotExistExceptionReturnsBadRequest()
    {
        Guid entityUid = Guid.NewGuid();
        decimal givenRating = 4.5m;

        mockRatingPersisterService
            .Setup(rps => rps.AddAsync(It.IsAny<IGivenRating>()))
            .Throws(() => new EntityDoesNotExistException(entityUid));

        RatingsController ratingsController = new RatingsController(
            givenRatingFactory,
            consideredRatingFactory,
            mockRatingPersisterService.Object,
            mockRatingCalculationService.Object,
            cachingService,
            logger,
            mockHostApplicationLifeTime.Object
        );

        var createRatingRequest = new CreateRatingRequest()
        {
            EntityUid = entityUid,
            EntityType = "Property",
            RaterUid = null,
            Rating = givenRating,
        };

        var actionResult = ratingsController.CreateRating(createRatingRequest).GetAwaiter().GetResult();
        Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));

        var badRequestObjectResult = (BadRequestObjectResult)actionResult;
        Assert.IsInstanceOfType(badRequestObjectResult.Value, typeof(string));
        Assert.IsNotNull(badRequestObjectResult.Value);
        Assert.IsTrue(((string)badRequestObjectResult.Value).ToLower().Contains(entityUid.ToString().ToLower()));
    }

    [TestMethod]
    public void StoredRatingValueInvalidExceptionReturnsInternalServerError()
    {
        Guid entityUid = Guid.NewGuid();
        decimal givenRating = 4.5m;
        decimal storedRating = 3.2m;

        mockRatingPersisterService
            .Setup(rps => rps.AddAsync(It.IsAny<IGivenRating>()))
            .Throws(() => new StoredRatingValueInvalidException(storedRating));

        RatingsController ratingsController = new RatingsController(
            givenRatingFactory,
            consideredRatingFactory,
            mockRatingPersisterService.Object,
            mockRatingCalculationService.Object,
            cachingService,
            logger,
            mockHostApplicationLifeTime.Object
        );

        var createRatingRequest = new CreateRatingRequest()
        {
            EntityUid = entityUid,
            EntityType = "Property",
            RaterUid = null,
            Rating = givenRating,
        };

        var actionResult = ratingsController.CreateRating(createRatingRequest).GetAwaiter().GetResult();
        Assert.IsInstanceOfType(actionResult, typeof(StatusCodeResult));

        var statusCodeResult = (StatusCodeResult)actionResult;
        Assert.AreEqual(HttpStatusCode.InternalServerError, (HttpStatusCode)statusCodeResult.StatusCode);
    }

    [TestMethod]
    public void EntityInvalidExceptionReturnsInternalServerError()
    {
        Guid entityUid = Guid.NewGuid();
        decimal givenRating = 4.5m;

        mockRatingPersisterService
            .Setup(rps => rps.AddAsync(It.IsAny<IGivenRating>()))
            .Throws(() => new EntityInvalidException(entityUid));

        RatingsController ratingsController = new RatingsController(
            givenRatingFactory,
            consideredRatingFactory,
            mockRatingPersisterService.Object,
            mockRatingCalculationService.Object,
            cachingService,
            logger,
            mockHostApplicationLifeTime.Object
        );

        var createRatingRequest = new CreateRatingRequest()
        {
            EntityUid = entityUid,
            EntityType = "Property",
            RaterUid = null,
            Rating = givenRating,
        };

        var actionResult = ratingsController.CreateRating(createRatingRequest).GetAwaiter().GetResult();
        Assert.IsInstanceOfType(actionResult, typeof(StatusCodeResult));

        var statusCodeResult = (StatusCodeResult)actionResult;
        Assert.AreEqual(HttpStatusCode.InternalServerError, (HttpStatusCode)statusCodeResult.StatusCode);
    }
}
