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
    private readonly int cacheExpirationScanFrequency;

    public int CacheExpirationMinutes { get { return cacheExpirationMinutes; } }
    public int CacheExpirationScanFrequency { get { return cacheExpirationScanFrequency; } }

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

        if (!int.TryParse(configuration["cacheExpirationScanFrequency"], out cacheExpirationScanFrequency))
            cacheExpirationScanFrequency = 5;
    }
}
