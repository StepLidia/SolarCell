# Solar Profitability Estimator

A small ASP.NET Core Web API for estimating residential solar panel profitability.

The project demonstrates:
- ASP.NET Core Web API
- Controller-based architecture
- Dependency injection
- Service layer abstraction
- Unit testing with xUnit
- Integration testing with WebApplicationFactory
- Swagger/OpenAPI
- HTTP request testing with `.http` files

## Technologies

- C#
- .NET 10
- ASP.NET Core
- xUnit
- Swagger / Swashbuckle

## Project Structure

```text
backend: solar-profitability-estimator/
                                    src/SolarProfitabilityEstimator.Api/
                                    tests/
frontend:
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
- Docker support