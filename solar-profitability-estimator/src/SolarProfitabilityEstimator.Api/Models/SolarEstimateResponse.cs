namespace SolarProfitabilityEstimator.Api.Models;

/// <summary>
/// Represents a response for estimating the profitability of a solar panel installation.
/// </summary>
public sealed class SolarEstimateResponse
{
    public decimal AnnualProductionKwh { get; set; }

    public decimal AnnualSavings { get; set; }

    public decimal PaybackYears { get; set; }
}