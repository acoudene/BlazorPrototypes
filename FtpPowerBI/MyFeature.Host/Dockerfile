# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MyFeature.Host/MyFeature.Host.csproj", "MyFeature.Host/"]
COPY ["MyFeature.Api/MyFeature.Api.csproj", "MyFeature.Api/"]
COPY ["Core.Api/Core.Api.csproj", "Core.Api/"]
COPY ["Core.Data/Core.Data.csproj", "Core.Data/"]
COPY ["Core.Dtos/Core.Dtos.csproj", "Core.Dtos/"]
COPY ["MyFeature.Data/MyFeature.Data.csproj", "MyFeature.Data/"]
COPY ["MyFeature.Dtos/MyFeature.Dtos.csproj", "MyFeature.Dtos/"]
COPY ["MyFeature.Data.MongoDb/MyFeature.Data.MongoDb.csproj", "MyFeature.Data.MongoDb/"]
COPY ["Core.Data.MongoDb/Core.Data.MongoDb.csproj", "Core.Data.MongoDb/"]
COPY ["FtpPowerBI.ServiceDefaults/FtpPowerBI.ServiceDefaults.csproj", "FtpPowerBI.ServiceDefaults/"]
RUN dotnet restore "./MyFeature.Host/MyFeature.Host.csproj"
COPY . .
WORKDIR "/src/MyFeature.Host"
RUN dotnet build "./MyFeature.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyFeature.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyFeature.Host.dll"]