FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MassTransitRepro.Consumer/MassTransitRepro.Consumer.csproj", "MassTransitRepro.Consumer/"]
RUN dotnet restore "MassTransitRepro.Consumer/MassTransitRepro.Consumer.csproj"
COPY . .
WORKDIR "/src/MassTransitRepro.Consumer"
RUN dotnet build "MassTransitRepro.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MassTransitRepro.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MassTransitRepro.Consumer.dll"]
