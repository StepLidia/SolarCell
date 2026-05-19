using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarProfitabilityEstimator.Domain;

public class SolarEstimateConfiguration : IEntityTypeConfiguration<SolarEstimate>
{
    public void Configure(EntityTypeBuilder<SolarEstimate> entity)
    {
        entity.Property(x => x.AnnualProductionKwh).HasPrecision(18, 2);
        entity.Property(x => x.SystemSizeKw).HasPrecision(18, 2);
        entity.Property(x => x.AnnualSavings).HasPrecision(18, 2);
    }
}