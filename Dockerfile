FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

WORKDIR /src/WebAPI
RUN dotnet restore WebAPI.csproj
RUN dotnet publish WebAPI.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS="http://+:8080"
ENV DATASOURCE_URL="Host=host.docker.internal;Port=5432;Database=financeiradb;Username=postgres;Password=postgres"

ENTRYPOINT ["dotnet", "WebAPI.dll"]