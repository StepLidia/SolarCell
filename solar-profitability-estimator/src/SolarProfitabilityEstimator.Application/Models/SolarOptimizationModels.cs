namespace SolarProfitabilityEstimator.Application.Optimization;

public record SolarOptimizationInput(
    double Latitude,
    double Longitude,
    double PanelAreaM2,
    double PanelEfficiency,
    IReadOnlyList<WeatherSample> Weather);

public record WeatherSample(
    DateTime Timestamp,
    double DirectNormalIrradiance,
    double DiffuseHorizontalIrradiance,
    double GlobalHorizontalIrradiance,
    double TemperatureC);

public record SolarOptimizationResult(
    double TiltAngle,
    double AzimuthAngle,
    double EnergyKWh);