namespace SolarProfitabilityEstimator.Api.Models;

/// <summary>
/// Represents a request for estimating the profitability of a solar panel installation.
/// Contains parameters such as system size, annual yield, electricity price, installation cost, and self-consumption rate.
/// </summary>
public sealed class SolarEstimateRequest
{
    public decimal SystemSizeKw { get; set; }
    public decimal AnnualYieldPerKw { get; set; }
    public decimal ElectricityPricePerKwh { get; set; }
    public decimal InstallationCost { get; set; }
    public decimal SelfConsumptionRate { get; set; }
}