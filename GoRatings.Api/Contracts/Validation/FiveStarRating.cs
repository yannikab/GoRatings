using System.ComponentModel.DataAnnotations;

namespace GoRatings.Api.Contracts.Validation;

public class FiveStarRating : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return false;

        if (value is not decimal)
            return false;

        decimal d = (decimal)value;

        if (d < 0 || d > 5)
            return false;

        d += d;

        if (d != Math.Floor(d))
            return false;

        return true;
    }

    public override string FormatErrorMessage(string name)
    {
        return string.Format($"{name} must be a multiple of 0.5 within the range of 0 to 5, inclusive.");
    }
}
