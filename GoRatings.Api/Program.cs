using GoRatings.Api.Contracts.Ratings;
using GoRatings.Services.Caching;
using GoRatings.Services.Interfaces.Services;
using GoRatings.Services.OldRatingsCleanup;
using GoRatings.Services.PropertyPersister;
using GoRatings.Services.RatingCalculation;
using GoRatings.Services.RatingPersister;
using GoRatings.Services.RealEstateAgentPersister;

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
