using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

namespace GoRatings.Services;

internal class Settings
{
    private readonly int cacheExpirationMinutes;
    private readonly int cacheExpirationScanFrequencyMinutes;
    public int CacheExpirationMinutes { get { return cacheExpirationMinutes; } }
    public int CacheExpirationScanFrequencyMinutes { get { return cacheExpirationScanFrequencyMinutes; } }

    private readonly int ratingsCleanupAgeDays;
    private readonly int ratingsCleanupIntervalMinutes;
    public int RatingsCleanupAgeDays { get { return ratingsCleanupAgeDays; } }
    public int RatingsCleanupIntervalMinutes { get { return ratingsCleanupIntervalMinutes; } }

    private static readonly Settings instance = new();

    public static Settings Instance
    {
        get { return instance; }
    }

    private Settings()
    {
        IConfigurationRoot configuration =
            new ConfigurationBuilder()
            .AddJsonFile("GoRatings.Services.config.json", false, true)
            .Build();

        if (!int.TryParse(configuration["cacheExpirationMinutes"], out cacheExpirationMinutes))
            cacheExpirationMinutes = 60;

        if (!int.TryParse(configuration["cacheExpirationScanFrequencyMinutes"], out cacheExpirationScanFrequencyMinutes))
            cacheExpirationScanFrequencyMinutes = 10;

        if (!int.TryParse(configuration["ratingsCleanupAgeDays"], out ratingsCleanupAgeDays))
            ratingsCleanupAgeDays = 100;

        if (!int.TryParse(configuration["ratingsCleanupIntervalMinutes"], out ratingsCleanupIntervalMinutes))
            ratingsCleanupIntervalMinutes = 60;
    }
}
