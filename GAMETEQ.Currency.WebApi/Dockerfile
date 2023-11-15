FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GAMETEQ.Currency.WebApi/GAMETEQ.Currency.WebApi.csproj", "GAMETEQ.Currency.WebApi/"]
RUN dotnet restore "GAMETEQ.Currency.WebApi/GAMETEQ.Currency.WebApi.csproj"
COPY . .
WORKDIR "/src/GAMETEQ.Currency.WebApi"
RUN dotnet build "GAMETEQ.Currency.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GAMETEQ.Currency.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GAMETEQ.Currency.WebApi.dll"]
