# ---------- BUILD STAGE ----------

# Bruger .NET SDK image fordi vi skal bygge projektet
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

# Arbejdsmappe inde i containeren
WORKDIR /app

# Kopierer hele frontend-projektet ind i containeren
COPY . .

# Gendanner NuGet pakker
RUN dotnet restore "FitLife.Frontend.csproj"

# Bygger og publicerer projektet
# -c Release = release build
# -o = output mappe
RUN dotnet publish \
    "FitLife.Frontend.csproj" \ 
    -c Release \
    -o /app/publish



# ---------- RUNTIME STAGE ----------

# Bruger mindre runtime image (ingen SDK)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine

# Arbejdsmappe i runtime-containeren
WORKDIR /app

# Kopierer kun det publicerede output fra build-stage
COPY --from=build /app/publish .

# Fortæller ASP.NET at den skal lytte på port 8080
ENV ASPNETCORE_URLS=http://+:8080

# Starter frontend-applikationen
ENTRYPOINT ["dotnet", "FitLife.Frontend.dll"]