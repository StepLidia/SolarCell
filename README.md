# Solar Profitability Estimator

A small ASP.NET Core Web API for estimating residential solar panel profitability.

The project demonstrates:
- ASP.NET Core Web API
- Dependency injection
- EF Core and migrations
- Unit testing with xUnit
- Integration testing with WebApplicationFactory
- Swagger/OpenAPI
- HTTP request testing with `.http` files
- Usage of dependabot
- CI/CD pipelines with Docker image and GitHub Container Registry

## Technologies

- C#
- .NET 10
- ASP.NET Core
- xUnit
- Swagger / Swashbuckle

## Project Structure

```text
backend C#: solar-profitability-estimator
frontend React:
```

## Features

The API calculates:
- annual solar energy production
- estimated yearly savings
- estimated payback period

## Run the Application

```powershell
dotnet restore
dotnet run --project src/SolarProfitabilityEstimator.Api
```
## Future Improvements

Possible future extensions:
- degradation modelling
- battery storage calculations
- solar irradiation API integration
- database persistence
- deployment to Azure Cloud