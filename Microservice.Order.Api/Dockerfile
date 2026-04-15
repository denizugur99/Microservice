# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Microservice.Order.Api/Microservice.Order.Api.csproj", "Microservice.Order.Api/"]
COPY ["Microservice.Order.Application/Microservice.Order.Application.csproj", "Microservice.Order.Application/"]
COPY ["Microservice.Bus/Microservice.Bus.csproj", "Microservice.Bus/"]
COPY ["Microservice.Order.Domain/Microservice.Order.Domain.csproj", "Microservice.Order.Domain/"]
COPY ["Shared/Microservices.Shared.csproj", "Shared/"]
COPY ["Microservice.Order.Persistence/Microservice.Order.Persistence.csproj", "Microservice.Order.Persistence/"]
RUN dotnet restore "./Microservice.Order.Api/Microservice.Order.Api.csproj"
COPY . .
WORKDIR "/src/Microservice.Order.Api"
RUN dotnet build "./Microservice.Order.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Microservice.Order.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservice.Order.Api.dll"]