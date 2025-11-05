
# Etapa aonde a imagem do sdk é baixada
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build

# Copia apenas o csproj e restaura dependências
COPY *.csproj ./
RUN dotnet restore *.csproj

# Copia todo o código
COPY . ./
RUN dotnet publish *.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS="http://+:8080"

# Variáveis de ambiente
ENV DATASOURCE_URL="Host=host.docker.internal;Port=5432;Database=financeiradb;Username=postgres;Password=postgres"
ENV ASPNETCORE_URLS="http://+:8080"

# Comando de inicialização
ENTRYPOINT ["dotnet", "financeira.dll"]