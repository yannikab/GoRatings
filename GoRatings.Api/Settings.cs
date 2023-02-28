using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

namespace GoRatings.Api;

internal class Settings
{
    private readonly int pastDays;
    public int PastDays { get { return pastDays; } }

    private readonly int cacheExpirationMinutes;
    private readonly int cacheExpirationScanFrequencyMinutes;
    public int CacheExpirationMinutes { get { return cacheExpirationMinutes; } }
    public int CacheExpirationScanFrequencyMinutes { get { return cacheExpirationScanFrequencyMinutes; } }

    private readonly int ratingsCleanupAgeDays;
    private readonly int ratingsCleanupIntervalMinutes;
    private readonly int ratingsCleanupStartupDelayMinutes;
    public int RatingsCleanupAgeDays { get { return ratingsCleanupAgeDays; } }
    public int RatingsCleanupIntervalMinutes { get { return ratingsCleanupIntervalMinutes; } }
    public int RatingsCleanupStartupDelayMinutes { get { return ratingsCleanupStartupDelayMinutes; } }

    private static readonly Settings instance = new();

    public static Settings Instance
    {
        get { return instance; }
    }

    private Settings()
    {
        IConfigurationRoot configuration =
            new ConfigurationBuilder()
            .AddJsonFile("GoRatings.Api.config.json", false, true)
            .Build();

        if (!int.TryParse(configuration["pastDays"], out pastDays))
            pastDays = 100;

        if (!int.TryParse(configuration["cacheExpirationMinutes"], out cacheExpirationMinutes))
            cacheExpirationMinutes = 60;

        if (!int.TryParse(configuration["cacheExpirationScanFrequencyMinutes"], out cacheExpirationScanFrequencyMinutes))
            cacheExpirationScanFrequencyMinutes = 10;

        if (!int.TryParse(configuration["ratingsCleanupAgeDays"], out ratingsCleanupAgeDays))
            ratingsCleanupAgeDays = 100;

        if (!int.TryParse(configuration["ratingsCleanupIntervalMinutes"], out ratingsCleanupIntervalMinutes))
            ratingsCleanupIntervalMinutes = 60;

        if (!int.TryParse(configuration["ratingsCleanupStartupDelayMinutes"], out ratingsCleanupStartupDelayMinutes))
            ratingsCleanupStartupDelayMinutes = 15;
    }
}
