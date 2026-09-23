# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0.402@sha256:df188b6cb642450d83d3d857a992bbbef248212fd456dcb9ae582ddd1bb96dce AS build
WORKDIR /app

# Copiar archivos de proyecto usando la estructura real (sin redundancia /src/src/)
# Asumiendo que el contexto es la raíz del repositorio
COPY ["src/EventHouse.Management.Api/EventHouse.Management.Api.csproj", "src/EventHouse.Management.Api/"]
COPY ["src/EventHouse.Management.Application/EventHouse.Management.Application.csproj", "src/EventHouse.Management.Application/"]
COPY ["src/EventHouse.Management.Domain/EventHouse.Management.Domain.csproj", "src/EventHouse.Management.Domain/"]
COPY ["src/EventHouse.ShareKernel/EventHouse.ShareKernel.csproj", "src/EventHouse.ShareKernel/"]
COPY ["src/EventHouse.Management.Infrastructure/EventHouse.Management.Infrastructure.csproj", "src/EventHouse.Management.Infrastructure/"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]
COPY ["src/EventHouse.Management.Api/packages.lock.json", "src/EventHouse.Management.Api/"]
COPY ["src/EventHouse.Management.Application/packages.lock.json", "src/EventHouse.Management.Application/"]
COPY ["src/EventHouse.Management.Domain/packages.lock.json", "src/EventHouse.Management.Domain/"]
COPY ["src/EventHouse.Management.Infrastructure/packages.lock.json", "src/EventHouse.Management.Infrastructure/"]
COPY ["src/EventHouse.ShareKernel/packages.lock.json", "src/EventHouse.ShareKernel/"]

# Restaurar dependencias
RUN dotnet restore "src/EventHouse.Management.Api/EventHouse.Management.Api.csproj" --locked-mode

# Copiar el resto del código
COPY . .

# Construir la aplicación
RUN dotnet build "src/EventHouse.Management.Api/EventHouse.Management.Api.csproj" -c Release --no-restore -o /app/build

# 2. Publish Stage
FROM build AS publish
RUN dotnet publish "src/EventHouse.Management.Api/EventHouse.Management.Api.csproj" -c Release --no-restore -o /app/publish /p:UseAppHost=false

# 3. Final Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0.8@sha256:30d8619d9a4f68508d9b17fc2088e857e629d3f9ceaaf57c22d6747f7326d89e AS final
WORKDIR /app

# Copiar binarios publicados
COPY --from=publish /app/publish .

# Crear carpeta de persistencia
RUN mkdir -p /app/Persistence && chmod 777 /app/Persistence

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "EventHouse.Management.Api.dll"]
