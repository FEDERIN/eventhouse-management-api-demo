# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files to leverage Docker layer caching
# Context is assumed to be the repository root
COPY ["EventHouse.Management.Api/EventHouse.Management.Api.csproj", "EventHouse.Management.Api/"]
COPY ["EventHouse.Management.Application/EventHouse.Management.Application.csproj", "EventHouse.Management.Application/"]
COPY ["EventHouse.Management.Domain/EventHouse.Management.Domain.csproj", "EventHouse.Management.Domain/"]
COPY ["EventHouse.ShareKernel/EventHouse.ShareKernel.csproj", "EventHouse.ShareKernel/"]
COPY ["EventHouse.Management.Infrastructure/EventHouse.Management.Infrastructure.csproj", "EventHouse.Management.Infrastructure/"]

# Restore dependencies from NuGet.org
# Your library FGutierrez.Core.Observability is now public, so no local source is needed
RUN dotnet restore "EventHouse.Management.Api/EventHouse.Management.Api.csproj"

# Copy the rest of the source code
COPY . .

# Cleanup Windows-generated temporary folders (bin/obj) that might have been copied
RUN find . -type d \( -name "obj" -o -name "bin" \) -exec rm -rf {} +

# Build the application in Release mode
WORKDIR "/src/EventHouse.Management.Api"
RUN dotnet build "EventHouse.Management.Api.csproj" -c Release -o /app/build

# 2. Publish Stage
FROM build AS publish
RUN dotnet publish "EventHouse.Management.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 3. Final Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy optimized binaries from the publish stage
COPY --from=publish /app/publish .

# Create the folder where the DB will live and give permissions
RUN mkdir -p /app/Persistence && chmod 777 /app/Persistence

# The database file will "appear" here thanks to Docker Compose
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "EventHouse.Management.Api.dll"]