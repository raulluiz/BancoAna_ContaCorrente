# Stage base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Stage build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BancoAna_ContaCorrente/BancoAna_ContaCorrente.csproj", "BancoAna_ContaCorrente/"]
RUN dotnet restore "./BancoAna_ContaCorrente/BancoAna_ContaCorrente.csproj"
COPY . .
WORKDIR "/src/BancoAna_ContaCorrente"
RUN dotnet build "./BancoAna_ContaCorrente.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BancoAna_ContaCorrente.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BancoAna_ContaCorrente.dll"]
