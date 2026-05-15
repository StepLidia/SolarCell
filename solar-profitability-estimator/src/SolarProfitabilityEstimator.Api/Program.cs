using Azure.Monitor.OpenTelemetry.AspNetCore;
using SolarProfitabilityEstimator.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ISolarProfitabilityCalculator, SolarProfitabilityCalculator>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry().UseAzureMonitor();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();