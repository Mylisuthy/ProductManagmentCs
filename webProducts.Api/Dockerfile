# Usa una imagen base para la compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia los archivos del proyecto (incluye el .sln y todos los .csproj)
# Asumiendo que el contexto de build es la raíz (ProductManagmentCs/)
COPY ["ProductManager.sln", "."] 
COPY ["webProducts.Api/webProducts.Api.csproj", "webProducts.Api/"]
COPY ["webProducts.Application/webProducts.Application.csproj", "webProducts.Application/"]
COPY ["webProducts.Domain/webProducts.Domain.csproj", "webProducts.Domain/"]
COPY ["webProducts.Infrastructure/webProducts.Infrastructure.csproj", "webProducts.Infrastructure/"]

# Restaura las dependencias (el .sln permite resolver todas las dependencias)
RUN dotnet restore "ProductManager.sln"

# Copia el resto del código
COPY . .

# Publica la aplicación final
WORKDIR /src/webProducts.Api
RUN dotnet publish "webProducts.Api.csproj" -c Release -o /app/publish

# Crea la imagen de ejecución final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

# Copia los archivos publicados
COPY --from=build /app/publish .

# Define el punto de entrada de la aplicación
ENTRYPOINT ["dotnet", "webProducts.Api.dll"]