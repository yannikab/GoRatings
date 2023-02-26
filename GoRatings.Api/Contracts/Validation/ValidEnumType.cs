using System.ComponentModel.DataAnnotations;

namespace GoRatings.Api.Contracts.Validation;

public class ValidEnumType : ValidationAttribute
{
    private readonly Type enumType;

    public ValidEnumType(Type enumType)
    {
        if (!enumType.IsEnum)
            throw new ArgumentException($"Type {nameof(enumType)} is not an enum type");

        this.enumType = enumType;
    }

    public override bool IsValid(object? value)
    {
        if (value is not string s)
            return false;

        if (!Enum.TryParse(enumType, s, true, out object? _))
            return false;

        return true;
    }

    public override string FormatErrorMessage(string name)
    {
        return string.Format($"Valid values for {name} are: {string.Join(", ", enumType.GetEnumNames())}");
    }
}
