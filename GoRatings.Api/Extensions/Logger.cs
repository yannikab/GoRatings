using System.Diagnostics.CodeAnalysis;

namespace GoRatings.Api;

[SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>")]

public static partial class Extensions
{
    public static void Trace<T>(this ILogger<T> logger, string message)
    {
        logger.LogTrace(message);
    }

    public static void Debug<T>(this ILogger<T> logger, string message)
    {
        logger.LogDebug(message);
    }

    public static void Info<T>(this ILogger<T> logger, string message)
    {
        logger.LogInformation(message);
    }

    public static void Warn<T>(this ILogger<T> logger, string message)
    {
        logger.LogWarning(message);
    }

    public static void Error<T>(this ILogger<T> logger, string message)
    {
        logger.LogError(message);
    }

    public static void Critical<T>(this ILogger<T> logger, string message)
    {
        logger.LogCritical(message);
    }

    public static void Trace<T>(this ILogger<T> logger, Exception ex)
    {
        logger.LogTrace(ex, string.Empty);
    }

    public static void Debug<T>(this ILogger<T> logger, Exception ex)
    {
        logger.LogDebug(ex, string.Empty);
    }

    public static void Info<T>(this ILogger<T> logger, Exception ex)
    {
        logger.LogInformation(ex, string.Empty);
    }

    public static void Warn<T>(this ILogger<T> logger, Exception ex)
    {
        logger.LogWarning(ex, string.Empty);
    }

    public static void Error<T>(this ILogger<T> logger, Exception ex)
    {
        logger.LogError(ex, string.Empty);
    }

    public static void Critical<T>(this ILogger<T> logger, Exception ex)
    {
        logger.LogCritical(ex, string.Empty);
    }
}
