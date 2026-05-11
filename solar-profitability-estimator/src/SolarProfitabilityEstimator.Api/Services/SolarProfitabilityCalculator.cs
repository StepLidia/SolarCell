using SolarProfitabilityEstimator.Api.Models;

namespace SolarProfitabilityEstimator.Api.Services;

/// <summary>
/// Provides functionality to calculate the profitability of a solar panel installation based on input parameters such as system size, annual yield, electricity price, installation cost, and self-consumption rate.
/// </summary>
public sealed class SolarProfitabilityCalculator : ISolarProfitabilityCalculator
{
    /// <summary>
    /// Calculates the annual production, savings, and payback period for a solar panel installation based on the provided request parameters.
    /// </summary>
    /// <param name="request">Model of request.</param>
    /// <returns>Model of response.</returns>
    /// <exception cref="ArgumentException"></exception>
    public SolarEstimateResponse Calculate(SolarEstimateRequest request)
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

        decimal annualProductionKwh = request.SystemSizeKw * request.AnnualYieldPerKw;
        decimal annualSavings = annualProductionKwh * request.SelfConsumptionRate * request.ElectricityPricePerKwh;
        decimal paybackYears = request.InstallationCost / annualSavings;

        return new SolarEstimateResponse
        {
            AnnualProductionKwh = Math.Round(annualProductionKwh, 2),
            AnnualSavings = Math.Round(annualSavings, 2),
            PaybackYears = Math.Round(paybackYears, 2)
        };
    }
}