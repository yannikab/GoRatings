using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

namespace GoRatings.DataAccess;

internal class Settings
{
    private readonly string connectionString;

    public string ConnectionString { get { return connectionString; } }

    private static readonly Settings instance = new();

    public static Settings Instance
    {
        get { return instance; }
    }

    private Settings()
    {
        IConfigurationRoot configuration =
            new ConfigurationBuilder()
            .AddJsonFile("GoRatings.DataAccess.config.json", false, true)
            .Build();

        if (!bool.TryParse(configuration["developmentMode"], out bool developmentMode))
            developmentMode = true;

        connectionString = configuration[developmentMode ? "connectionStringLocal" : "connectionStringLive"];
    }
}
