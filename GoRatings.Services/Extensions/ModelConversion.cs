using GoRatings.DataAccess.Models;
using GoRatings.Services.PropertyPersister.Interfaces;
using GoRatings.Services.RatingCalculation.Interfaces;
using GoRatings.Services.RatingCalculation.Models;
using GoRatings.Services.RatingPersister.Interfaces;
using GoRatings.Services.RealEstateAgentPersister.Interfaces;

namespace GoRatings.Services;

public static partial class Extensions
{
    public static Rating ToRating(this IGivenRating givenRating, long entityId)
    {
        return new Rating()
        {
            EntityId = entityId,
            Rater = givenRating.RaterUid,
            Value = givenRating.Rating,
            CreatedDt = DateTime.UtcNow,
            IsActive = true,
        };
    }

    public static Property ToProperty(this IGivenProperty givenProperty)
    {
        return new Property()
        {
            Address = givenProperty.Address,
            City = givenProperty.City,
            State = givenProperty.State,
            ZipCode = givenProperty.ZipCode,
            SquareFootage = givenProperty.SquareFootage,
            YearBuilt = givenProperty.YearBuilt,
            ListingPrice = givenProperty.ListingPrice,
            Entity = new Entity()
            {
                Uid = Guid.NewGuid(),
                Code = givenProperty.Code,
                Description = givenProperty.Description,
                CreatedDt = DateTime.UtcNow,
                IsActive = true,
            },
        };
    }

    public static RealEstateAgent ToRealEstateAgent(this IGivenRealEstateAgent givenRealEstateAgent)
    {
        return new RealEstateAgent()
        {
            FirstName = givenRealEstateAgent.FirstName,
            LastName = givenRealEstateAgent.LastName,
            Email = givenRealEstateAgent.Email,
            Phone = givenRealEstateAgent.Phone,
            LicenseNumber = givenRealEstateAgent.LicenseNumber,
            BrokerageFirm = givenRealEstateAgent.BrokerageFirm,
            Entity = new Entity()
            {
                Uid = Guid.NewGuid(),
                Code = givenRealEstateAgent.Code,
                Description = givenRealEstateAgent.Description,
                CreatedDt = DateTime.UtcNow,
                IsActive = true,
            },
        };
    }

    public static IConsideredRating ToConsideredRating(this IStoredRating storedRating)
    {
        return new ConsideredRating()
        {
            Rating = storedRating.Rating,
            CreatedDT = storedRating.CreatedDt,
            IsAnonymous = !storedRating.RaterUid.HasValue,
        };
    }
}
