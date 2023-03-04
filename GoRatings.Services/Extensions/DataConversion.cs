using GoRatings.DataAccess.Models;
using GoRatings.Services.PropertyPersister.Interfaces;
using GoRatings.Services.PropertyPersister.Models;
using GoRatings.Services.RatingPersister.Exceptions;
using GoRatings.Services.RatingPersister.Interfaces;
using GoRatings.Services.RatingPersister.Models;
using GoRatings.Services.RealEstateAgentPersister.Interfaces;
using GoRatings.Services.RealEstateAgentPersister.Models;

namespace GoRatings.Services;

public static partial class Extensions
{
    public static IStoredRating ToStoredRating(this Rating rating)
    {
        return new StoredRating()
        {
            EntityUid = rating.Entity.Uid,
            EntityType = rating.Entity.GetEntityType(),
            RaterUid = rating.Rater,
            Rating = rating.Value,
            CreatedDt = rating.CreatedDt,
        };
    }

    public static IStoredProperty ToStoredProperty(this Property property)
    {
        Entity entity = property.Entity!;

        return new StoredProperty()
        {
            EntityUid = entity.Uid,
            CreatedDT = entity.CreatedDt,
            IsActive = entity.IsActive,
            Code = entity.Code,
            Description = entity.Description,
            Address = property.Address,
            City = property.City,
            State = property.State,
            ZipCode = property.ZipCode,
            SquareFootage = property.SquareFootage,
            YearBuilt = property.YearBuilt,
            ListingPrice = property.ListingPrice,
        };
    }

    public static IStoredRealEstateAgent ToStoredRealEstateAgent(this RealEstateAgent realEstateAgent)
    {
        Entity entity = realEstateAgent.Entity!;

        return new StoredRealEstateAgent()
        {
            EntityUid = entity.Uid,
            CreatedDT = entity.CreatedDt,
            IsActive = entity.IsActive,
            Code = entity.Code,
            Description = entity.Description,
            FirstName = realEstateAgent.FirstName,
            LastName = realEstateAgent.LastName,
            Email = realEstateAgent.Email,
            Phone = realEstateAgent.Phone,
            LicenseNumber = realEstateAgent.LicenseNumber,
            BrokerageFirm = realEstateAgent.BrokerageFirm,
        };
    }

    public static EntityType GetEntityType(this Entity entity)
    {
        if (entity.PropertyId.HasValue && entity.RealEstateAgentId.HasValue)
            throw new EntityInvalidException(entity.Uid);

        if (entity.PropertyId.HasValue)
            return EntityType.Property;

        if (entity.RealEstateAgentId.HasValue)
            return EntityType.RealEstateAgent;

        throw new EntityInvalidException(entity.Uid);
    }
}
