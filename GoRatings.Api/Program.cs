using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using GoRatings.Api;
using GoRatings.Api.Contracts.Ratings;
using GoRatings.Api.Services.RatingsCleanup;
using GoRatings.Services.Caching;
using GoRatings.Services.Caching.Interfaces;
using GoRatings.Services.PropertyPersister;
using GoRatings.Services.PropertyPersister.Exceptions;
using GoRatings.Services.PropertyPersister.Interfaces;
using GoRatings.Services.PropertyPersister.Models;
using GoRatings.Services.RatingCalculation;
using GoRatings.Services.RatingCalculation.Exceptions;
using GoRatings.Services.RatingCalculation.Interfaces;
using GoRatings.Services.RatingCalculation.Models;
using GoRatings.Services.RatingPersister;
using GoRatings.Services.RatingPersister.Exceptions;
using GoRatings.Services.RatingPersister.Interfaces;
using GoRatings.Services.RatingPersister.Models;
using GoRatings.Services.RatingsCleanup;
using GoRatings.Services.RatingsCleanup.Interfaces;
using GoRatings.Services.RealEstateAgentPersister;
using GoRatings.Services.RealEstateAgentPersister.Exceptions;
using GoRatings.Services.RealEstateAgentPersister.Interfaces;
using GoRatings.Services.RealEstateAgentPersister.Models;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.OpenApi.Models;

using NLog;
using NLog.Web;

using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GoRatings API",
        Version = "v1",
        Description = "This is a simple API for a five-star based rating calculation system",
        Contact = new OpenApiContact
        {
            Name = "Ioannis Kampylafkas",
            Email = "ioannis.kampylafkas@gmail.com",
            Url = new Uri("https://github.com/yannikab")
        },
        License = new OpenApiLicense
        {
            Name = "Microsoft Public License (MS-PL)",
            Url = new Uri("https://opensource.org/licenses/MS-PL")
        }
    });

    options.EnableAnnotations();

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddSingleton<IGivenPropertyFactory, GivenPropertyFactory>();
builder.Services.AddSingleton<IGivenRealEstateAgentFactory, GivenRealEstateAgentFactory>();
builder.Services.AddSingleton<IGivenRatingFactory, GivenRatingFactory>();
builder.Services.AddSingleton<IConsideredRatingFactory, ConsideredRatingFactory>();

builder.Services.AddScoped<IPropertyPersisterService, PropertyPersisterService>();
builder.Services.AddScoped<IRealEstateAgentPersisterService, RealEstateAgentPersisterService>();
builder.Services.AddScoped<IRatingPersisterService, RatingPersisterService>();

builder.Services.AddSingleton<IRatingCalculationService, RatingCalculationService>();

var cacheExpiration = TimeSpan.FromMinutes(Settings.Instance.CacheExpirationMinutes);
var cacheExpirationScanFrequency = TimeSpan.FromMinutes(Settings.Instance.CacheExpirationScanFrequencyMinutes);
builder.Services.AddSingleton<ICachingService<Guid, OverallRatingResponse>>(provider => new MemoryCachingService<Guid, OverallRatingResponse>(cacheExpiration, cacheExpirationScanFrequency));

builder.Services.AddSingleton<IRatingsCleanupService, RatingsCleanupService>();
builder.Services.AddHostedService<RatingsCleanupHostedService>();

var app = builder.Build();

app.UseExceptionHandler(configure =>
{
    configure.Run(async context =>
    {
        var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();

        if (exceptionHandler == null)
            return;

        Exception exception = exceptionHandler.Error;

        if (exception == null)
            return;

        ILogger<Program> log = context.RequestServices.GetService<ILogger<Program>>() ?? new NullLoggerFactory().CreateLogger<Program>();

        int statusCode;
        string message;

        try
        {
            throw exception;
        }
        catch (Exception ex) when
        (
            ex is EntityDoesNotExistException ||
            ex is EntityUidTypeMismatchException ||
            ex is GivenRatingValueInvalidException)
        {
            log.Info(ex);

            statusCode = 400;
            message = ex.Message;
        }
        catch (Exception ex) when
        (
            ex is PropertyDoesNotExistException ||
            ex is RealEstateAgentDoesNotExistException)
        {
            log.Info(ex);

            statusCode = 404;
            message = ex.Message;
        }
        catch (Exception ex) when
        (
            ex is EntityInvalidException ||
            ex is StoredRatingValueInvalidException)
        {
            log.Warn(ex);

            statusCode = 500;
            message = "A rating persister error has occurred.";
        }
        catch (Exception ex) when
        (
            ex is RatingCalculationException ||
            ex is OverallRatingInvalidException)
        {
            log.Warn(ex);

            statusCode = 500;
            message = "A rating calculation error has occurred.";
        }
        catch (Exception ex) when (!ex.IsCritical())
        {
            log.Error(ex);

            statusCode = 500;
            message = "An error has occurred.";
        }
        catch (Exception ex)
        {
            log.Critical(ex);

            var hostApplicationLifetime = context.RequestServices.GetService<IHostApplicationLifetime>();

            if (hostApplicationLifetime != null)
                hostApplicationLifetime.StopApplication();

            statusCode = 500;
            message = "A fatal error has occurred.";
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json; charset=utf-8";

        var jsonResponse = JsonSerializer.Serialize(message);

        await context.Response.WriteAsync(jsonResponse);
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.DocExpansion(DocExpansion.None);
        options.DefaultModelRendering(ModelRendering.Model);
        options.DefaultModelExpandDepth(3);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//Console.WriteLine("Done.");
//Console.ReadKey();
