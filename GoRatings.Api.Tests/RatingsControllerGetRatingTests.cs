using System.Diagnostics.CodeAnalysis;
using System.Net;

using GoRatings.Api.Contracts.Ratings;
using GoRatings.Api.Controllers;
using GoRatings.Services.Caching;
using GoRatings.Services.Caching.Interfaces;
using GoRatings.Services.RatingCalculation.Exceptions;
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
public class RatingsControllerGetRatingTests
{
    readonly IGivenRatingFactory givenRatingFactory = new GivenRatingFactory();
    readonly IConsideredRatingFactory consideredRatingFactory = new ConsideredRatingFactory();

    readonly Mock<IRatingPersisterService> mockRatingPersisterService = new Mock<IRatingPersisterService>();
    readonly Mock<IRatingCalculationService> mockRatingCalculationService = new Mock<IRatingCalculationService>();
    readonly ICachingService<Guid, OverallRatingResponse> cachingService = new MemoryCachingService<Guid, OverallRatingResponse>(TimeSpan.FromMinutes(60), TimeSpan.FromMinutes(10));
    readonly ILogger<RatingsController> logger = new NullLoggerFactory().CreateLogger<RatingsController>();
    readonly Mock<IHostApplicationLifetime> mockHostApplicationLifeTime = new Mock<IHostApplicationLifetime>();

    [TestMethod]
    public void ValidOverallRatingReturnsOk()
    {
        Guid entityUid = Guid.NewGuid();
        int consideredRatings = 100;
        decimal overallRating = 3.3m;

        mockRatingPersisterService
            .Setup(rps => rps.GetWithinPastDaysAsync(entityUid, It.IsAny<int>()))
            .Returns(Task.FromResult(Enumerable.Empty<IStoredRating>()));

        mockRatingCalculationService
            .Setup(rcs => rcs.CalculateOverallRatingAsync(Enumerable.Empty<IConsideredRating>(), It.IsAny<DateTime>(), It.IsAny<int>()))
            .Returns(Task.FromResult((IOverallRating)new OverallRating
            {
                CalculatedDT = DateTime.UtcNow,
                ConsideredRatings = consideredRatings,
                Rating = overallRating
            }));

        RatingsController ratingsController = new RatingsController(
            givenRatingFactory,
            consideredRatingFactory,
            mockRatingPersisterService.Object,
            mockRatingCalculationService.Object,
            cachingService,
            logger,
            mockHostApplicationLifeTime.Object
        );

        var actionResult = ratingsController.GetRating(entityUid).GetAwaiter().GetResult();
        Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));

        var okObjectResult = (OkObjectResult)actionResult;
        Assert.IsNotNull(okObjectResult.Value);
        Assert.IsInstanceOfType(okObjectResult.Value, typeof(OverallRatingResponse));

        var overallRatingResponse = (OverallRatingResponse)okObjectResult.Value;
        Assert.AreEqual(entityUid, overallRatingResponse.EntityUid);
        Assert.AreEqual(consideredRatings, overallRatingResponse.ConsideredRatings);
        Assert.AreEqual(overallRating, overallRatingResponse.Rating);
    }

    [TestMethod]
    public void NoRatingsReturnsNotFound()
    {
        Guid entityUid = Guid.NewGuid();

        mockRatingPersisterService
            .Setup(rps => rps.GetWithinPastDaysAsync(entityUid, It.IsAny<int>()))
            .Returns(Task.FromResult(Enumerable.Empty<IStoredRating>()));

        mockRatingCalculationService
            .Setup(rcs => rcs.CalculateOverallRatingAsync(Enumerable.Empty<IConsideredRating>(), It.IsAny<DateTime>(), It.IsAny<int>()))
            .Returns(Task.FromResult((IOverallRating)new OverallRating { CalculatedDT = DateTime.UtcNow, ConsideredRatings = 0, Rating = 0 }));

        RatingsController ratingsController = new RatingsController(
            givenRatingFactory,
            consideredRatingFactory,
            mockRatingPersisterService.Object,
            mockRatingCalculationService.Object,
            cachingService,
            logger,
            mockHostApplicationLifeTime.Object
        );

        var actionResult = ratingsController.GetRating(entityUid).GetAwaiter().GetResult();
        Assert.IsInstanceOfType(actionResult, typeof(NotFoundObjectResult));

        var notFoundObjectResult = (NotFoundObjectResult)actionResult;
        Assert.IsNotNull(notFoundObjectResult.Value);
        Assert.IsInstanceOfType(notFoundObjectResult.Value, typeof(Guid));
        Assert.AreEqual(entityUid, notFoundObjectResult.Value);
    }

    [TestMethod]
    public void EntityDoesNotExistExceptionReturnsBadRequest()
    {
        Guid entityUid = Guid.NewGuid();

        mockRatingPersisterService
            .Setup(rps => rps.GetWithinPastDaysAsync(entityUid, It.IsAny<int>()))
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

        var actionResult = ratingsController.GetRating(entityUid).GetAwaiter().GetResult();
        Assert.IsInstanceOfType(actionResult, typeof(BadRequestObjectResult));

        var badRequestObjectResult = (BadRequestObjectResult)actionResult;
        Assert.IsNotNull(badRequestObjectResult.Value);
        Assert.IsInstanceOfType(badRequestObjectResult.Value, typeof(string));
        Assert.IsTrue(((string)badRequestObjectResult.Value).ToLower().Contains(entityUid.ToString().ToLower()));
    }

    [TestMethod]
    public void StoredRatingValueInvalidExceptionReturnsInternalServerError()
    {
        Guid entityUid = Guid.NewGuid();

        mockRatingPersisterService
            .Setup(rps => rps.GetWithinPastDaysAsync(entityUid, It.IsAny<int>()))
            .Throws(() => new StoredRatingValueInvalidException(entityUid, 3.3m));

        RatingsController ratingsController = new RatingsController(
            givenRatingFactory,
            consideredRatingFactory,
            mockRatingPersisterService.Object,
            mockRatingCalculationService.Object,
            cachingService,
            logger,
            mockHostApplicationLifeTime.Object
        );

        var actionResult = ratingsController.GetRating(entityUid).GetAwaiter().GetResult();
        Assert.IsInstanceOfType(actionResult, typeof(StatusCodeResult));

        var statusCodeResult = (StatusCodeResult)actionResult;
        Assert.AreEqual(HttpStatusCode.InternalServerError, (HttpStatusCode)statusCodeResult.StatusCode);
    }

    [TestMethod]
    public void RatingCalculationExceptionReturnsInternalServerError()
    {
        Guid entityUid = Guid.NewGuid();

        mockRatingCalculationService
            .Setup(rcs => rcs.CalculateOverallRatingAsync(Enumerable.Empty<IConsideredRating>(), It.IsAny<DateTime>(), It.IsAny<int>()))
            .Throws<RatingCalculationException>();

        RatingsController ratingsController = new RatingsController(
            givenRatingFactory,
            consideredRatingFactory,
            mockRatingPersisterService.Object,
            mockRatingCalculationService.Object,
            cachingService,
            logger,
            mockHostApplicationLifeTime.Object
        );

        var actionResult = ratingsController.GetRating(entityUid).GetAwaiter().GetResult();
        Assert.IsInstanceOfType(actionResult, typeof(StatusCodeResult));

        var statusCodeResult = (StatusCodeResult)actionResult;
        Assert.AreEqual(HttpStatusCode.InternalServerError, (HttpStatusCode)statusCodeResult.StatusCode);
    }

    [TestMethod]
    public void EntityInvalidExceptionReturnsInternalServerError()
    {
        Guid entityUid = Guid.NewGuid();

        mockRatingPersisterService
            .Setup(rps => rps.GetWithinPastDaysAsync(entityUid, It.IsAny<int>()))
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

        var actionResult = ratingsController.GetRating(entityUid).GetAwaiter().GetResult();
        Assert.IsInstanceOfType(actionResult, typeof(StatusCodeResult));

        var statusCodeResult = (StatusCodeResult)actionResult;
        Assert.AreEqual(HttpStatusCode.InternalServerError, (HttpStatusCode)statusCodeResult.StatusCode);
    }
}
