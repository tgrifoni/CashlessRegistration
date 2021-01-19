FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CashlessRegistration.API/CashlessRegistration.API.csproj", "CashlessRegistration.API/"]
COPY ["CashlessRegistration.API.Domain/CashlessRegistration.API.Domain.csproj", "CashlessRegistration.API.Domain/"]
COPY ["CashlessRegistration.API.Infra.Data/CashlessRegistration.API.Infra.Data.csproj", "CashlessRegistration.API.Infra.Data/"]
RUN dotnet restore "CashlessRegistration.API/CashlessRegistration.API.csproj"
COPY . .
WORKDIR "/src/CashlessRegistration.API"
RUN dotnet build "CashlessRegistration.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CashlessRegistration.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CashlessRegistration.API.dll"]