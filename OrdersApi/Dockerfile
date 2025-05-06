# Consulte https://aka.ms/customizecontainer para aprender a personalizar su contenedor de depuración y cómo Visual Studio usa este Dockerfile para compilar sus imágenes para una depuración más rápida.

# Dependiendo del sistema operativo de las máquinas host que vayan a compilar o ejecutar los contenedores, puede que sea necesario cambiar la imagen especificada en la instrucción FROM.
# Para más información, consulte https://aka.ms/containercompat

# Esta fase se usa cuando se ejecuta desde VS en modo rápido (valor predeterminado para la configuración de depuración)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# Esta fase se usa para compilar el proyecto de servicio
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OrdersApi/OrdersApi.csproj", "OrdersApi/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Repository/Repository.csproj", "Repository/"]
RUN dotnet restore "./OrdersApi/OrdersApi.csproj"
COPY . .
WORKDIR "/src/OrdersApi"
RUN dotnet build "./OrdersApi.csproj" -c Release -o /app/build

# Esta fase se usa para publicar el proyecto de servicio que se copiará en la fase final.
FROM build AS publish
RUN dotnet publish "./OrdersApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Esta fase se usa en producción o cuando se ejecuta desde VS en modo normal (valor predeterminado cuando no se usa la configuración de depuración)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrdersApi.dll"]