FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/SolarProfitabilityEstimator.Api/SolarProfitabilityEstimator.Api.csproj", "SolarProfitabilityEstimator.Api/"]
COPY ["src/SolarProfitabilityEstimator.Domain/SolarProfitabilityEstimator.Domain.csproj", "SolarProfitabilityEstimator.Domain/"]
COPY ["src/SolarProfitabilityEstimator.Infrastructure/SolarProfitabilityEstimator.Infrastructure.csproj", "SolarProfitabilityEstimator.Infrastructure/"]

RUN dotnet restore "SolarProfitabilityEstimator.Api/SolarProfitabilityEstimator.Api.csproj"

COPY src/ .
RUN dotnet publish "SolarProfitabilityEstimator.Api/SolarProfitabilityEstimator.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "SolarProfitabilityEstimator.Api.dll"]