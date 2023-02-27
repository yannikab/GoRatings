using GoRatings.Api.Contracts.Ratings;
using GoRatings.Api.Interfaces.Services;
using GoRatings.Api.Services.Caching;
using GoRatings.Api.Services.OldRatingsCleanup;
using GoRatings.Api.Services.PropertyPersister;
using GoRatings.Api.Services.RatingCalculation;
using GoRatings.Api.Services.RatingPersister;
using GoRatings.Api.Services.RealEstateAgentPersister;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
