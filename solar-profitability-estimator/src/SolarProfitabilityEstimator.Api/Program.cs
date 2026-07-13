using Azure.Monitor.OpenTelemetry.AspNetCore;
using SolarProfitabilityEstimator.Application.Interfaces;
using SolarProfitabilityEstimator.Application.Services;
using SolarProfitabilityEstimator.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ISolarProfitabilityCalculator, SolarProfitabilityCalculator>();

builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);

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