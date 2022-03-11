FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["backend/city-service/city-api/city-api.csproj", "backend/city-service/city-api/"]
RUN dotnet restore "backend/city-service/city-api/city-api.csproj"
COPY . .
WORKDIR "/src/backend/city-service/city-api"
RUN dotnet build "city-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "city-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "city-api.dll"]
