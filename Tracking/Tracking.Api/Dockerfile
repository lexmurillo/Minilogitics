
# Etapa 1: compilar la API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore "Tracking.Api.csproj"
RUN dotnet publish "Tracking.Api.csproj" -c Release -o /app/publish --no-restore

# Etapa 2: ejecutar la API
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Tracking.Api.dll"]