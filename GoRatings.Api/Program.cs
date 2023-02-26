using GoRatings.Api.Contracts.Ratings;
using GoRatings.Services.Caching.Interfaces;
using GoRatings.Services.Caching.Service;
using GoRatings.Services.OldRatingsCleanup.Service;
using GoRatings.Services.RatingCalculation.Interfaces;
using GoRatings.Services.RatingCalculation.Service;
using GoRatings.Services.RatingPersister.Interfaces;
using GoRatings.Services.RatingPersister.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRatingPersisterService, RatingPersister>();
builder.Services.AddScoped<IRatingCalculationService, RatingCalculationService>();
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
