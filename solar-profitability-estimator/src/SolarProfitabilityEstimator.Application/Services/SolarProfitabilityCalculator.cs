using System.Diagnostics;
using Microsoft.Extensions.Logging;
using SolarProfitabilityEstimator.Application.Interfaces;
using SolarProfitabilityEstimator.Application.Models;
using SolarProfitabilityEstimator.Application.Optimization;

namespace SolarProfitabilityEstimator.Application.Services;

/// <summary>
/// Provides functionality to calculate the profitability of a solar panel installation based on input parameters such as system size, annual yield, electricity price, installation cost, and self-consumption rate.
/// </summary>
public sealed class SolarProfitabilityCalculator : ISolarProfitabilityCalculator
{
    /// <summary>
    /// Repository to access solar-related data, if needed for future enhancements or calculations.
    /// </summary>
    private readonly ISolarRepository solarRepository;

    /// <summary>
    /// Logger to log intermediate information in calculations.
    /// </summary>
    private readonly ILogger<SolarProfitabilityCalculator> logger;

    public SolarProfitabilityCalculator(ISolarRepository solarRepository, ILogger<SolarProfitabilityCalculator> logger)
    {
        this.solarRepository = solarRepository;
        this.logger = logger;
    }

    /// <inheritdoc />
    public async Task<SolarEstimateResponse> CalculateAsync(SolarEstimateRequest request)
    {
        if (request.SystemSizeKw <= 0)
        {
            throw new ArgumentException("System size must be greater than zero.");
        }

        if (request.AnnualYieldPerKw <= 0)
        {
            throw new ArgumentException("Annual yield must be greater than zero.");
        }

        if (request.ElectricityPricePerKwh <= 0)
        {
            throw new ArgumentException("Electricity price must be greater than zero.");
        }

        if (request.InstallationCost <= 0)
        {
            throw new ArgumentException("Installation cost must be greater than zero.");
        }

        if (request.SelfConsumptionRate < 0 || request.SelfConsumptionRate > 1)
        {
            throw new ArgumentException("Self-consumption rate must be between 0 and 1.");
        }

        var stopwatch = Stopwatch.StartNew();

        decimal annualProductionKwh = request.SystemSizeKw * request.AnnualYieldPerKw;
        decimal annualSavings = annualProductionKwh * request.SelfConsumptionRate * request.ElectricityPricePerKwh;
        decimal paybackYears = annualSavings > 0 ? request.InstallationCost / annualSavings : 0;

        stopwatch.Stop();

        this.logger.LogInformation("Calculated solar profitability for {SystemSize}kW in {ElapsedMs}ms.", request.SystemSizeKw, stopwatch.ElapsedMilliseconds);

        var result = new SolarEstimateResponse
        {
            AnnualProductionKwh = Math.Round(annualProductionKwh, 2),
            AnnualSavings = Math.Round(annualSavings, 2),
            PaybackYears = Math.Round(paybackYears, 2),
        };

        var dbEntry = new Domain.Entities.SolarEstimate
        {
            Id = Guid.NewGuid(),
            TrackingId = request.ClientId,
            SystemSizeKw = request.SystemSizeKw,
            AnnualProductionKwh = result.AnnualProductionKwh,
            AnnualSavings = result.AnnualSavings,
        };

        await this.solarRepository.SaveEstimateAsync(dbEntry);

        this.logger.LogInformation("Saved solar estimate with client ID {ClientId}.", request.ClientId);

        return result;
    }

    /// <inheritdoc />
    public SolarOptimizationResult OptimizeSolarPanelOrientation(SolarOptimizationInput input)
    {
        throw new NotImplementedException();
    }

    private static double CalculateTotalEnergy(SolarOptimizationInput input, double tiltAngle, double azimuthAngle)
    {
        if (input.Weather.Count < 2)
        {
            throw new ArgumentException("At least two weather samples are required to calculate total energy.");
        }

        for (int i = 0; i < input.Weather.Count; i++)
        {
            var sample = input.Weather[i];
            var nextSample = input.Weather[i + 1];

            var timeDifferenceHours = (nextSample.Timestamp - sample.Timestamp).TotalHours;

            if (timeDifferenceHours <= 0)
            {
                throw new ArgumentException("Weather samples must be in a chronological order and have a positive time difference.");
            }

            // calculate sun position based on latitude, longitude and time of year
            (double sunAltitude, double sunAzimuth) = (0.0, 0.0);

            if (sunAltitude < 0)
            {
                continue; // sun is below the horizon
            }

            var incidenceCosine = CalculateIncidenceCosine(tiltAngle, azimuthAngle, sunAltitude, sunAzimuth); // assuming sun azimuth is 0 for simplicity

            var directIrradiance = sample.DirectNormalIrradiance * incidenceCosine;
            var diffuseIrradiance = sample.DiffuseHorizontalIrradiance * (1 + Math.Cos(double.DegreesToRadians(tiltAngle))) / 2;
            var reflectedIrradiance = sample.GlobalHorizontalIrradiance * 0.2 * (1 - Math.Cos(double.DegreesToRadians(tiltAngle))) / 2; // assuming 20% ground reflectivity

            var panelIrradiance = Math.Max(0, directIrradiance + diffuseIrradiance + reflectedIrradiance);
        }

        return 0.0;
    }

    private static double CalculateIncidenceCosine(double panelTiltAngle, double panelAzimuthAngle, double sunAltitudeAngle, double sunAzimuthAngle)
    {
        // Convert angles from degrees to radians
        double tiltRad = double.DegreesToRadians(panelTiltAngle);
        double azimuthRad = double.DegreesToRadians(panelAzimuthAngle);
        double sunAltitudeRad = double.DegreesToRadians(sunAltitudeAngle);
        double sunAzimuthRad = double.DegreesToRadians(sunAzimuthAngle);

        // Calculate the cosine of the incidence angle
        double cosIncidence = (Math.Sin(sunAltitudeRad) * Math.Cos(tiltRad)) +
                              (Math.Cos(sunAltitudeRad) * Math.Sin(tiltRad) * Math.Cos(sunAzimuthRad - azimuthRad));

        return Math.Max(0, cosIncidence);
    }
}