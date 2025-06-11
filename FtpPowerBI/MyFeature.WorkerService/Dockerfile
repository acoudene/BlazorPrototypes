# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
ENV MyEntity_API_BASEADDRESS=https://localhost:7059/api/MyEntity/
ENV TimerOptions__Period=00:02:00

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["MyFeature.WorkerService/MyFeature.WorkerService.csproj", "MyFeature.WorkerService/"]
COPY ["FtpPowerBI.ServiceDefaults/FtpPowerBI.ServiceDefaults.csproj", "FtpPowerBI.ServiceDefaults/"]
COPY ["MyFeature.Proxies.FtpClient/MyFeature.Proxies.Ftp.csproj", "MyFeature.Proxies.FtpClient/"]
COPY ["MyFeature.Dtos/MyFeature.Dtos.csproj", "MyFeature.Dtos/"]
COPY ["Core.Dtos/Core.Dtos.csproj", "Core.Dtos/"]
COPY ["MyFeature.Proxies/MyFeature.Proxies.csproj", "MyFeature.Proxies/"]
COPY ["Core.Proxying/Core.Proxying.csproj", "Core.Proxying/"]
RUN dotnet restore "./MyFeature.WorkerService/MyFeature.WorkerService.csproj"
COPY . .
WORKDIR "/src/MyFeature.WorkerService"
RUN dotnet build "./MyFeature.WorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyFeature.WorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyFeature.WorkerService.dll", "--periodic true"]
