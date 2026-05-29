# ---------- BUILD ----------

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

# Arbejdsmappe i container
WORKDIR /app

# Kopierer hele projektet ind
COPY . .

# Henter NuGet pakker
RUN dotnet restore "FacilityServiceAPI.csproj"

# Bygger og publicerer projektet
RUN dotnet publish \
    "FacilityServiceAPI.csproj" \
    -c Release \
    -o /app/published-app



# ---------- RUNTIME ----------

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime

WORKDIR /app

# Kopierer det færdige build over
COPY --from=build /app/published-app .

# Starter applikationen
ENTRYPOINT ["dotnet", "FacilityServiceAPI.dll"]