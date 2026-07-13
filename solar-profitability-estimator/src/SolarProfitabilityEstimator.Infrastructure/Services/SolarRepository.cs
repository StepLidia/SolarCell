using SolarProfitabilityEstimator.Application.Interfaces;
using SolarProfitabilityEstimator.Domain.Entities;

namespace SolarProfitabilityEstimator.Infrastructure.Services;

/// <summary>
/// Implements the ISolarRepository interface to handle the persistence of solar estimates to the database.
/// </summary>
public class SolarRepository : ISolarRepository
{
    private readonly SolarDbContext dbContext;

    public SolarRepository(SolarDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task SaveEstimateAsync(SolarEstimate estimate)
    {
        await this.dbContext.SolarEstimates.AddAsync(estimate);
        await this.dbContext.SaveChangesAsync();
    }
}