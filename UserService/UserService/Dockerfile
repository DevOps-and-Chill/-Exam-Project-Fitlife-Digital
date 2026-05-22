FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

WORKDIR /app

COPY . .

RUN dotnet restore "UserServiceAPI.csproj"

RUN dotnet publish \
    "UserServiceAPI.csproj" \
    -c Release \
    -o /app/published-app


FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime

WORKDIR /app

COPY --from=build /app/published-app .

ENTRYPOINT ["dotnet", "UserServiceAPI.dll"]