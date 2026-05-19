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

    /// <summary>
    /// Configures the model by applying entity configurations from the assembly.
    /// This method is called by the framework when the model is being created, allowing us to define how our entities map to the database schema.
    /// </summary>
    /// <param name="modelBuilder">The ModelBuilder used to configure entity mappings and relationships.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SolarDbContext).Assembly);
    }
}
