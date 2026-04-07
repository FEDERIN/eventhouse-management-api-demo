# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy NuGetLocal to the root
COPY ["NuGetLocal/", "/NuGetLocal/"]

# Copy project files
COPY ["eventhouse-management-api/EventHouse.Management.Api/EventHouse.Management.Api.csproj", "eventhouse-management-api/EventHouse.Management.Api/"]
COPY ["eventhouse-management-api/EventHouse.Management.Application/EventHouse.Management.Application.csproj", "eventhouse-management-api/EventHouse.Management.Application/"]
COPY ["eventhouse-management-api/EventHouse.Management.Domain/EventHouse.Management.Domain.csproj", "eventhouse-management-api/EventHouse.Management.Domain/"]
COPY ["eventhouse-management-api/EventHouse.ShareKernel/EventHouse.ShareKernel.csproj", "eventhouse-management-api/EventHouse.ShareKernel/"]
COPY ["eventhouse-management-api/EventHouse.Management.Infrastructure/EventHouse.Management.Infrastructure.csproj", "eventhouse-management-api/EventHouse.Management.Infrastructure/"]

# Add local source
RUN dotnet nuget add source /NuGetLocal --name LocalRepo

# Restore
RUN dotnet restore "eventhouse-management-api/EventHouse.Management.Api/EventHouse.Management.Api.csproj" --source "https://api.nuget.org/v3/index.json" --source "/NuGetLocal"

# Copy everything
COPY . .

# --- CRITICAL FIX: Delete Windows-generated bin/obj folders ---
RUN find . -type d \( -name "obj" -o -name "bin" \) -exec rm -rf {} +
# Delete any existing nuget.config
RUN find . -name "nuget.config" -type f -delete || true

# Build
WORKDIR "/src/eventhouse-management-api/EventHouse.Management.Api"
# Note: Removed --no-restore because we cleaned the obj folders
RUN dotnet build "EventHouse.Management.Api.csproj" -c Release -o /app/build

# 2. Publish Stage
FROM build AS publish
RUN dotnet publish "EventHouse.Management.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 3. Final Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /app/Data && chmod 777 /app/Data

EXPOSE 8080
ENTRYPOINT ["dotnet", "EventHouse.Management.Api.dll"]