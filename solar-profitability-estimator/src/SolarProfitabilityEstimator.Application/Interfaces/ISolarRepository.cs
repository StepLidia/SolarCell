using SolarProfitabilityEstimator.Domain.Entities;

namespace SolarProfitabilityEstimator.Application.Interfaces;

/// <summary>
/// Defines an interface for a repository that handles the persistence of data to database.
/// </summary>
public interface ISolarRepository
{
    /// <summary>
    /// Saves the provided solar estimate to the database asynchronously.
    /// </summary>
    /// <param name="estimate">The solar estimate to save.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SaveEstimateAsync(SolarEstimate estimate);
}