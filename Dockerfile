FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ECommerceOrderManagement.API/ECommerceOrderManagement.API.csproj", "ECommerceOrderManagement.API/"]
COPY ["ECommerceOrderManagement.Core/ECommerceOrderManagement.Core.csproj", "ECommerceOrderManagement.Core/"]
COPY ["ECommerceOrderManagement.Infrastructure/ECommerceOrderManagement.Infrastructure.csproj", "ECommerceOrderManagement.Infrastructure/"]
RUN dotnet restore "ECommerceOrderManagement.API/ECommerceOrderManagement.API.csproj"
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
COPY . .
WORKDIR "/src/ECommerceOrderManagement.API"
RUN dotnet build "ECommerceOrderManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerceOrderManagement.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src /src
COPY --from=build /root/.dotnet/tools /root/.dotnet/tools
ENV PATH="${PATH}:/root/.dotnet/tools"
ENTRYPOINT ["dotnet", "ECommerceOrderManagement.API.dll"] 