using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SolarProfitabilityEstimator.Api.Models;
using SolarProfitabilityEstimator.Api.Services;

namespace SolarProfitabilityEstimator.Tests;

/// <summary>
/// Contains unit tests for the SolarProfitabilityCalculator to verify that it correctly calculates the annual production, savings,
/// and payback period based on given input parameters. The test uses a known set of input values and asserts that the output matches the expected results.
/// </summary>
public sealed class ProfitabilityTests
{
    /// <summary>
    /// Instance of the SolarProfitabilityCalculator to be tested.
    /// It is initialized with a NullLogger to avoid logging during tests.
    /// </summary>
    private readonly SolarProfitabilityCalculator calculator;

    public ProfitabilityTests()
    {
        ILogger<SolarProfitabilityCalculator> logger = NullLogger<SolarProfitabilityCalculator>.Instance;

        this.calculator = new SolarProfitabilityCalculator(logger);
    }

    [Fact]
    public void Calculate_WithValidInput_ReturnsExpectedPayback()
    {
        // Arrange
        var request = new SolarEstimateRequest
        {
            SystemSizeKw = 8,
            AnnualYieldPerKw = 1100,
            ElectricityPricePerKwh = 0.32m,
            InstallationCost = 22000,
            SelfConsumptionRate = 0.65m
        };

        // Act
        SolarEstimateResponse result = this.calculator.Calculate(request);

        // Assert
        Assert.Equal(8800m, result.AnnualProductionKwh);
        Assert.Equal(1830.40m, result.AnnualSavings);
        Assert.Equal(12.02m, result.PaybackYears);
    }

    [Fact]
    public void Calculate_WithNegativeInput_ExpectException()
    {
        // Arrange
        var request = new SolarEstimateRequest
        {
            SystemSizeKw = 8,
            AnnualYieldPerKw = 1100,
            ElectricityPricePerKwh = -0.32m,
            InstallationCost = 22000,
            SelfConsumptionRate = -0.65m
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => this.calculator.Calculate(request));
    }

    [Fact]
    public void Calculate_NoAnnualSavings_ExpectSuccess()
    {
        // Arrange
        var request = new SolarEstimateRequest
        {
            SystemSizeKw = 8,
            AnnualYieldPerKw = 1100,
            ElectricityPricePerKwh = 0.22m,
            InstallationCost = 22000,
            SelfConsumptionRate = 0.0m
        };

        // Act
        SolarEstimateResponse result = this.calculator.Calculate(request);

        // Assert
        Assert.Equal(8800m, result.AnnualProductionKwh);
        Assert.Equal(0.0m, result.AnnualSavings);
        Assert.Equal(0.0m, result.PaybackYears);
    }
}
