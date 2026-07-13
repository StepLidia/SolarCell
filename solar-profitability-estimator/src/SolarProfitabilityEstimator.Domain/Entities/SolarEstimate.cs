namespace SolarProfitabilityEstimator.Domain.Entities;

/// <summary>
/// Entity to store solar calculation results in the database.
/// </summary>
public class SolarEstimate
{
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the tracking identifier associated with the solar estimate.
    /// </summary>
    public Guid TrackingId { get; set; }

    public decimal SystemSizeKw { get; set; }

    public decimal AnnualProductionKwh { get; set; }

    public decimal AnnualSavings { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the solar estimate was created UTC time.
    /// </summary>
    public DateTime DateCreated { get; set; }
}
