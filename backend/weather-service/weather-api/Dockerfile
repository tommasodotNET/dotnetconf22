FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ./weather-api.csproj backend/weather-service/weather-api/
RUN dotnet restore backend/weather-service/weather-api/weather-api.csproj
COPY . backend/weather-service/weather-api
WORKDIR /src/backend/weather-service/weather-api
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "weather-api.dll"]
