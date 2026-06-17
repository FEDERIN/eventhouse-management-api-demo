# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivos de proyecto usando la estructura real (sin redundancia /src/src/)
# Asumiendo que el contexto es la raíz del repositorio
COPY ["src/EventHouse.Management.Api/EventHouse.Management.Api.csproj", "src/EventHouse.Management.Api/"]
COPY ["src/EventHouse.Management.Application/EventHouse.Management.Application.csproj", "src/EventHouse.Management.Application/"]
COPY ["src/EventHouse.Management.Domain/EventHouse.Management.Domain.csproj", "src/EventHouse.Management.Domain/"]
COPY ["src/EventHouse.ShareKernel/EventHouse.ShareKernel.csproj", "src/EventHouse.ShareKernel/"]
COPY ["src/EventHouse.Management.Infrastructure/EventHouse.Management.Infrastructure.csproj", "src/EventHouse.Management.Infrastructure/"]

# Restaurar dependencias
RUN dotnet restore "src/EventHouse.Management.Api/EventHouse.Management.Api.csproj"

# Copiar el resto del código
COPY . .

# Construir la aplicación
RUN dotnet build "src/EventHouse.Management.Api/EventHouse.Management.Api.csproj" -c Release -o /app/build

# 2. Publish Stage
FROM build AS publish
RUN dotnet publish "src/EventHouse.Management.Api/EventHouse.Management.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 3. Final Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copiar binarios publicados
COPY --from=publish /app/publish .

# Crear carpeta de persistencia
RUN mkdir -p /app/Persistence && chmod 777 /app/Persistence

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "EventHouse.Management.Api.dll"]