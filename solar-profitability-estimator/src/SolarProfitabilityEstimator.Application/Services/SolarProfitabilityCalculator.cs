using System.Diagnostics;
using Microsoft.Extensions.Logging;
using SolarProfitabilityEstimator.Application.Interfaces;
using SolarProfitabilityEstimator.Application.Models;

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

    /// <summary>
    /// Calculates the annual production, savings, and payback period for a solar panel installation based on the provided request parameters.
    /// </summary>
    /// <param name="request">Model of request.</param>
    /// <returns>Model of response.</returns>
    /// <exception cref="ArgumentException">Thrown when one or more input parameters are invalid.</exception>
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
}