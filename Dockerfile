# syntax=mirror.gcr.io/docker/dockerfile:1.7-labs
#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
COPY --parents ./src/**/*.csproj .
RUN dotnet restore "./src/DP-backend/DP-backend.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./DP-backend/DP-backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DP-backend/DP-backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS="http://*:80"
ENTRYPOINT ["dotnet", "DP-backend.dll"]
