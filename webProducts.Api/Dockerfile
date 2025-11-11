# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Etapa build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["webProducts.Api/webProducts.Api.csproj", "webProducts.Api/"]
COPY ["webProducts.Application/webProducts.Application.csproj", "webProducts.Application/"]
COPY ["webProducts.Domain/webProducts.Domain.csproj", "webProducts.Domain/"]
COPY ["webProducts.Infrastructure/webProducts.Infrastructure.csproj", "webProducts.Infrastructure/"]
RUN dotnet restore "webProducts.Api/webProducts.Api.csproj"
COPY . .
WORKDIR "/src/webProducts.Api"
RUN dotnet build "webProducts.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "webProducts.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "webProducts.Api.dll"]
ENV ASPNETCORE_ENVIRONMENT=Production