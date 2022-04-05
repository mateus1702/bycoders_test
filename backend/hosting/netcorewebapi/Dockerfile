#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["backend/hosting/netcorewebapi/netcorewebapi.csproj", "backend/hosting/netcorewebapi/"]
RUN dotnet restore "backend/hosting/netcorewebapi/netcorewebapi.csproj"
COPY . .
WORKDIR "/src/backend/hosting/netcorewebapi"
RUN dotnet build "netcorewebapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "netcorewebapi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "netcorewebapi.dll"]