FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MassTransitRepro.Publisher/MassTransitRepro.Publisher.csproj", "MassTransitRepro.Publisher/"]
RUN dotnet restore "MassTransitRepro.Publisher/MassTransitRepro.Publisher.csproj"
COPY . .
WORKDIR "/src/MassTransitRepro.Publisher"
RUN dotnet build "MassTransitRepro.Publisher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MassTransitRepro.Publisher.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MassTransitRepro.Publisher.dll"]
