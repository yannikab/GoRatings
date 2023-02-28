using System.ComponentModel.DataAnnotations;

namespace GoRatings.Api.Contracts.Validation;

public class FiveStarRatingAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return false;

        if (value is not decimal)
            return false;

        decimal d = (decimal)value;

        return d.IsValidFiveStarRating();
    }

    public override string FormatErrorMessage(string name)
    {
        return string.Format($"{name} must be a multiple of 0.5 within the range of 0 to 5, inclusive.");
    }
}
