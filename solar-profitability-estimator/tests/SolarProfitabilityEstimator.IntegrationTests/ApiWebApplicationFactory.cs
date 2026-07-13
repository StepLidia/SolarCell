using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SolarProfitabilityEstimator.Infrastructure;

/// <summary>
/// Provides a custom WebApplicationFactory for integration testing the SolarProfitabilityEstimator API.
/// </summary>
public sealed class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private SqliteConnection? connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IDbContextOptionsConfiguration<SolarDbContext>>();
            services.RemoveAll<DbContextOptions<SolarDbContext>>();
            services.RemoveAll<SolarDbContext>();

            this.connection = new SqliteConnection("DataSource=:memory:");
            this.connection.Open();

            services.AddDbContext<SolarDbContext>(options =>
            {
                options.UseSqlite(this.connection);
            });

            using ServiceProvider serviceProvider = services.BuildServiceProvider();
            using IServiceScope scope = serviceProvider.CreateScope();

            SolarDbContext dbContext = scope.ServiceProvider.GetRequiredService<SolarDbContext>();

            dbContext.Database.EnsureCreated();
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.connection?.Dispose();
        }

        base.Dispose(disposing);
    }
}