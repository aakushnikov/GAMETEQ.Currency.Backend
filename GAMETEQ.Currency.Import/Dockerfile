FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GAMETEQ.Currency.Import/GAMETEQ.Currency.Import.csproj", "GAMETEQ.Currency.Import/"]
RUN dotnet restore "GAMETEQ.Currency.Import/GAMETEQ.Currency.Import.csproj"
COPY . .
WORKDIR "/src/GAMETEQ.Currency.Import"
RUN dotnet build "GAMETEQ.Currency.Import.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GAMETEQ.Currency.Import.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GAMETEQ.Currency.Import.dll"]
