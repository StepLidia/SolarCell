using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.EntityFrameworkCore;
using SolarProfitabilityEstimator.Api.Services;
using SolarProfitabilityEstimator.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SolarDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SolarDb"));
});

builder.Services.AddSingleton<ISolarProfitabilityCalculator, SolarProfitabilityCalculator>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Azure Monitor OpenTelemetry integration
string? appInsightsConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];

if (!string.IsNullOrEmpty(appInsightsConnectionString))
{
    builder.Services.AddOpenTelemetry().UseAzureMonitor();
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();