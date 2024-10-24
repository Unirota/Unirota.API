FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["Unirota.sln", "./"]
COPY ["src/Unirota.API/Unirota.API.csproj", "src/Unirota.API/"]
COPY ["src/Unirota.Application/Unirota.Application.csproj", "src/Unirota.Application/"]
COPY ["src/Unirota.Domain/Unirota.Domain.csproj", "src/Unirota.Domain/"]
COPY ["src/Unirota.Infrastructure/Unirota.Infrastructure.csproj", "src/Unirota.Infrastructure/"]
COPY ["src/Unirota.Migrations/Unirota.Migrations.csproj", "src/Unirota.Migrations/"]
COPY ["src/Unirota.Shared/Unirota.Shared.csproj", "src/Unirota.Shared/"]
COPY ["tests/Unirota.UnitTests/Unirota.UnitTests.csproj", "tests/Unirota.UnitTests/"]

RUN dotnet restore

COPY . .

RUN dotnet build src/Unirota.API/Unirota.API.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish src/Unirota.API/Unirota.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "Unirota.API.dll"]