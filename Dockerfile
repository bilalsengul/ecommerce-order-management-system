FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ECommerceOrderManagement.API/ECommerceOrderManagement.API.csproj", "ECommerceOrderManagement.API/"]
COPY ["ECommerceOrderManagement.Core/ECommerceOrderManagement.Core.csproj", "ECommerceOrderManagement.Core/"]
COPY ["ECommerceOrderManagement.Infrastructure/ECommerceOrderManagement.Infrastructure.csproj", "ECommerceOrderManagement.Infrastructure/"]
RUN dotnet restore "ECommerceOrderManagement.API/ECommerceOrderManagement.API.csproj"
RUN dotnet tool install --global dotnet-ef
COPY . .
WORKDIR "/src/ECommerceOrderManagement.API"
RUN dotnet build "ECommerceOrderManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerceOrderManagement.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerceOrderManagement.API.dll"] 