using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolarProfitabilityEstimator.Application.Interfaces;
using SolarProfitabilityEstimator.Infrastructure.Services;

namespace SolarProfitabilityEstimator.Infrastructure;

/// <summary>
/// Provides extension methods for registering infrastructure services in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SolarDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SolarDb"));
        });

        services.AddScoped<ISolarRepository, SolarRepository>();

        return services;
    }
}