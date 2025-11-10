FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["webProducts.Api/webProducts.Api.csproj", "webProducts.Api/"]
COPY ["webProducts.Application/webProducts.Application.csproj", "webProducts.Application/"]
COPY ["webProducts.Domain/webProducts.Domain.csproj", "webProducts.Domain/"]
COPY ["webProducts.Infrastructure/webProducts.Infrastructure.csproj", "webProducts.Infrastructure/"]
RUN dotnet restore "webProducts.Api/webProducts.Api.csproj"
COPY . .
WORKDIR "/src/webProducts.Api"
RUN dotnet build "./webProducts.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./webProducts.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "webProducts.Api.dll"]
