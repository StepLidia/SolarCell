using Microsoft.AspNetCore.Mvc;
using SolarProfitabilityEstimator.Api.Models;
using SolarProfitabilityEstimator.Api.Services;

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
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("profitability")]
    [ProducesResponseType<SolarEstimateResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<SolarEstimateResponse> Estimate([FromBody] SolarEstimateRequest request)
    {
        try
        {
            SolarEstimateResponse response = this.calculator.Calculate(request);

            return Ok(response);
        }
        catch (ArgumentException exception)
        {
            this.logger.LogError(exception, "Failed to calculate solar profitability: {ErrorMessage}", exception.Message);

            return BadRequest(new { error = exception.Message });
        }
    }
}