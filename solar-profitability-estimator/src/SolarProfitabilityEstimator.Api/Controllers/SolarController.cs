using Microsoft.AspNetCore.Mvc;
using SolarProfitabilityEstimator.Application.Interfaces;
using SolarProfitabilityEstimator.Application.Models;

namespace SolarProfitabilityEstimator.Api.Controllers;

/// <summary>
/// Controller responsible for handling requests related to solar panel profitability estimation.
/// It provides an endpoint to calculate the annual production, savings, and payback period based on the input parameters provided in the request body.
/// </summary>
[ApiController]
[Route("solar")]
public sealed class SolarController : ControllerBase
{
    private readonly ISolarProfitabilityCalculator calculator;

    private readonly ILogger<SolarController> logger;

    public SolarController(ISolarProfitabilityCalculator calculator, ILogger<SolarController> logger)
    {
        this.calculator = calculator;
        this.logger = logger;
    }

    /// <summary>
    /// Estimates the profitability of a solar panel installation based on the provided parameters in the request body.
    /// </summary>
    /// <param name="request">Body of a request.</param>
    /// <returns>Body of a response.</returns>
    [HttpPost("profitability")]
    [ProducesResponseType<SolarEstimateResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SolarEstimateResponse>> Estimate([FromBody] SolarEstimateRequest request)
    {
        try
        {
            SolarEstimateResponse response = await this.calculator.CalculateAsync(request);

            return this.Ok(response);
        }
        catch (ArgumentException exception)
        {
            this.logger.LogError(exception, "Failed to calculate solar profitability: {ErrorMessage}", exception.Message);

            return this.BadRequest(new { error = exception.Message });
        }
    }
}