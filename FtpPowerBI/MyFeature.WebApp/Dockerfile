# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MyFeature.WebApp/MyFeature.WebApp.csproj", "MyFeature.WebApp/"]
COPY ["MyFeature.Api.BackendForFrontend/MyFeature.Api.BackendForFrontend.csproj", "MyFeature.Api.BackendForFrontend/"]
COPY ["MyFeature.Proxies/MyFeature.Proxies.csproj", "MyFeature.Proxies/"]
COPY ["Core.Proxying/Core.Proxying.csproj", "Core.Proxying/"]
COPY ["Core.Dtos/Core.Dtos.csproj", "Core.Dtos/"]
COPY ["MyFeature.Dtos/MyFeature.Dtos.csproj", "MyFeature.Dtos/"]
COPY ["MyFeature.ViewObjects/MyFeature.ViewObjects.csproj", "MyFeature.ViewObjects/"]
COPY ["Core.ViewObjects/Core.ViewObjects.csproj", "Core.ViewObjects/"]
COPY ["FtpPowerBI.ServiceDefaults/FtpPowerBI.ServiceDefaults.csproj", "FtpPowerBI.ServiceDefaults/"]
COPY ["MyFeature.WebApp.Client/MyFeature.WebApp.Client.csproj", "MyFeature.WebApp.Client/"]
COPY ["Core.RazorComponents.Mud/Core.RazorComponents.Mud.csproj", "Core.RazorComponents.Mud/"]
COPY ["Core.RazorComponents/Core.RazorComponents.csproj", "Core.RazorComponents/"]
COPY ["MyFeature.Localization/MyFeature.Localization.csproj", "MyFeature.Localization/"]
COPY ["MyFeature.RazorComponents/MyFeature.RazorComponents.csproj", "MyFeature.RazorComponents/"]
COPY ["MyFeature.ViewObjects.Validation/MyFeature.ViewObjects.Validation.csproj", "MyFeature.ViewObjects.Validation/"]
COPY ["MyFeature.ViewModels/MyFeature.ViewModels.csproj", "MyFeature.ViewModels/"]
COPY ["Core.ViewModels/Core.ViewModels.csproj", "Core.ViewModels/"]
RUN dotnet restore "./MyFeature.WebApp/MyFeature.WebApp.csproj"
COPY . .
WORKDIR "/src/MyFeature.WebApp"
RUN dotnet build "./MyFeature.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyFeature.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyFeature.WebApp.dll"]