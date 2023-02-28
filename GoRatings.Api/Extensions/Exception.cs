using System.Security;

namespace GoRatings.Api;

public static partial class Extensions
{
    public static bool IsCritical(this Exception ex)
    {
        if (ex is OutOfMemoryException)
            return true;

        if (ex is AccessViolationException)
            return true;

        if (ex is StackOverflowException)
            return true;

        if (ex is SecurityException)
            return true;

        return false;
    }
}
