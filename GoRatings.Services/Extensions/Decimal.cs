namespace GoRatings.Services;

public static partial class Extensions
{
    public static bool IsValidFiveStarRating(this decimal d)
    {
        if (d < 0 || d > 5)
            return false;

        d += d;

        if (d != Math.Floor(d))
            return false;

        return true;
    }
}
