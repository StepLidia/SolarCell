using SolarProfitabilityEstimator.Api.Models;
using SolarProfitabilityEstimator.Api.Services;

namespace SolarProfitabilityEstimator.Tests;

/// <summary>
/// Contains unit tests for the SolarProfitabilityCalculator to verify that it correctly calculates the annual production, savings,
/// and payback period based on given input parameters. The test uses a known set of input values and asserts that the output matches the expected results.
/// </summary>
public sealed class ProfitabilityTests
{
    [Fact]
    public void Calculate_WithValidInput_ReturnsExpectedPayback()
    {
        // Arrange
        ISolarProfitabilityCalculator calculator = new SolarProfitabilityCalculator();

        var request = new SolarEstimateRequest
        {
            SystemSizeKw = 8,
            AnnualYieldPerKw = 1100,
            ElectricityPricePerKwh = 0.32m,
            InstallationCost = 22000,
            SelfConsumptionRate = 0.65m
        };

        // Act
        SolarEstimateResponse result = calculator.Calculate(request);

        // Assert
        Assert.Equal(8800m, result.AnnualProductionKwh);
        Assert.Equal(1830.40m, result.AnnualSavings);
        Assert.Equal(12.02m, result.PaybackYears);
    }

    [Fact]
    public void Calculate_WithNegativeInput_ExpectException()
    {
        // Arrange
        ISolarProfitabilityCalculator calculator = new SolarProfitabilityCalculator();

        var request = new SolarEstimateRequest
        {
            SystemSizeKw = 8,
            AnnualYieldPerKw = 1100,
            ElectricityPricePerKwh = -0.32m,
            InstallationCost = 22000,
            SelfConsumptionRate = -0.65m
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => calculator.Calculate(request));
    }

    [Fact]
    public void Calculate_NoAnnualSavings_ExpectSuccess()
    {
        // Arrange
        ISolarProfitabilityCalculator calculator = new SolarProfitabilityCalculator();

        var request = new SolarEstimateRequest
        {
            SystemSizeKw = 8,
            AnnualYieldPerKw = 1100,
            ElectricityPricePerKwh = 0.22m,
            InstallationCost = 22000,
            SelfConsumptionRate = 0.0m
        };

        // Act
        SolarEstimateResponse result = calculator.Calculate(request);

        // Assert
        Assert.Equal(8800m, result.AnnualProductionKwh);
        Assert.Equal(0.0m, result.AnnualSavings);
        Assert.Equal(0.0m, result.PaybackYears);
    }
}
