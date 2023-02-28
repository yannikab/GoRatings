using System.Reflection;
using System.Text.Json.Serialization;

using GoRatings.Api.Contracts.Ratings;
using GoRatings.Api.Interfaces.Services.Caching;
using GoRatings.Api.Interfaces.Services.PropertyPersister;
using GoRatings.Api.Interfaces.Services.RatingCalculation;
using GoRatings.Api.Interfaces.Services.RatingPersister;
using GoRatings.Api.Interfaces.Services.RealEstateAgentPersister;
using GoRatings.Api.Services.Caching;
using GoRatings.Api.Services.OldRatingsCleanup;
using GoRatings.Api.Services.PropertyPersister;
using GoRatings.Api.Services.RatingCalculation;
using GoRatings.Api.Services.RatingPersister;
using GoRatings.Api.Services.RealEstateAgentPersister;

using Microsoft.OpenApi.Models;

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

builder.Services.AddScoped<IRatingPersisterService, RatingPersisterService>();
builder.Services.AddScoped<IPropertyPersisterService, PropertyPersisterService>();
builder.Services.AddScoped<IRealEstateAgentPersisterService, RealEstateAgentPersisterService>();

builder.Services.AddSingleton<IRatingCalculationService, RatingCalculationService>();
builder.Services.AddSingleton<ICachingService<Guid, OverallRatingResponse>, MemoryCachingService<Guid, OverallRatingResponse>>();
builder.Services.AddHostedService<OldRatingsCleanupService>();

var app = builder.Build();

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
