using SolarProfitabilityEstimator.Application.Models;
using SolarProfitabilityEstimator.Application.Optimization;

namespace SolarProfitabilityEstimator.Application.Interfaces;

/// <summary>
/// Defines an interface for calculating the profitability of a solar panel installation.
/// </summary>
public interface ISolarProfitabilityCalculator
{
    /// <summary>
    /// Calculates the annual production, savings, and payback period for a solar panel installation based on the provided request parameters.
    /// </summary>
    /// <param name="request">Model of a request containing the parameters for the calculation.</param>
    /// <returns>A response containing the estimated profitability metrics.</returns>
    Task<SolarEstimateResponse> CalculateAsync(SolarEstimateRequest request);

    /// <summary>
    /// Optimizes the orientation of a solar panel installation based on the provided input parameters.
    /// </summary>
    /// <param name="input">Model of input parameters for the optimization.</param>
    /// <returns>A result containing the optimized tilt angle, azimuth angle, and estimated energy production.</returns>
    SolarOptimizationResult OptimizeSolarPanelOrientation(SolarOptimizationInput input);
}