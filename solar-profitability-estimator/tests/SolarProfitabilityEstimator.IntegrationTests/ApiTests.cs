using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using SolarProfitabilityEstimator.Api.Models;

namespace SolarProfitabilityEstimator.IntegrationTests;

/// <summary>
/// Contains integration tests for the Solar Profitability Estimator API to verify that the endpoints are functioning correctly and returning expected results.
/// The tests use a WebApplicationFactory to create an in-memory instance of the API and send HTTP requests to the endpoints, asserting on the responses received.
/// </summary>
public sealed class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client;

    public ApiTests(WebApplicationFactory<Program> factory)
    {
        client = factory.CreateClient();
    }

    [Fact]
    public async Task PostCalculation_ExpectSuccess()
    {
        var request = new SolarEstimateRequest
        {
            SystemSizeKw = 5,
            AnnualYieldPerKw = 1200,
            ElectricityPricePerKwh = 0.25m,
            InstallationCost = 15000,
            SelfConsumptionRate = 0.5m
        };

        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/solar/profitability",
            request);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<SolarEstimateResponse>();

        Assert.NotNull(result);
        Assert.True(result.AnnualProductionKwh > 0);
        Assert.True(result.AnnualSavings > 0);
        Assert.True(result.PaybackYears > 0);
    }
}
