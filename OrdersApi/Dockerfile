# Imagen base
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8183
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:8183

# Imagen final
FROM base AS final
WORKDIR /app
COPY publish/ .
ENTRYPOINT ["dotnet", "OrdersApi.dll"]


