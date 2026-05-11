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

    public SolarController(ISolarProfitabilityCalculator calculator)
    {
        this.calculator = calculator;
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
            SolarEstimateResponse response = calculator.Calculate(request);

            return Ok(response);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
    }
}