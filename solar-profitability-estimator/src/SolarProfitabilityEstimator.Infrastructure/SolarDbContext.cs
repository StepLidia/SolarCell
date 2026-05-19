using Microsoft.EntityFrameworkCore;
using SolarProfitabilityEstimator.Domain;

namespace SolarProfitabilityEstimator.Infrastructure;

/// <summary>
/// Database context for the solar profitability estimator application, responsible for managing the connection to the database and providing access to the SolarEstimates table.
/// </summary>
public class SolarDbContext : DbContext
{
    public SolarDbContext(DbContextOptions<SolarDbContext> options)
        : base(options)
    {
    }

    public DbSet<SolarEstimate> SolarEstimates => this.Set<SolarEstimate>();
}
