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
    }
}
