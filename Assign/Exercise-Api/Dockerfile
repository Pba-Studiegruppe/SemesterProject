# syntax=docker/dockerfile:1

# ---- Build stage -------------------------------------------------------------
# SDK-imaget bruges KUN til at restore/build/publish. Det ryger ikke med i
# det endelige image, så vi slæber ikke ~700MB SDK med ud i produktion.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Kopiér KUN csproj-filer først og restore. Dette lag caches af Docker så længe
# projektreferencer/pakker ikke ændres -> hurtigere rebuilds når kun kode ændres.
COPY ["Exercise-Api/Exercise-Api.csproj", "Exercise-Api/"]
COPY ["Exercise-Application/Exercise-Application.csproj", "Exercise-Application/"]
COPY ["Exercise-Domain/Exercise-Domain.csproj", "Exercise-Domain/"]
COPY ["Exercise-Infrastructure/Exercise-Infrastructure.csproj", "Exercise-Infrastructure/"]
RUN dotnet restore "Exercise-Api/Exercise-Api.csproj"

# Kopiér resten af kildekoden og publish.
COPY . .
WORKDIR "/src/Exercise-Api"
RUN dotnet publish "Exercise-Api.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# ---- Runtime stage -----------------------------------------------------------
# Slankt aspnet-runtime image (ingen SDK). $APP_UID er en indbygget non-root
# bruger i Microsofts images -> mindre angrebsflade.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Kør som non-root. Sættes efter WORKDIR så ejerskab er på plads.
USER $APP_UID

# Kun HTTP internt i container-netværket. HTTPS termineres typisk af en
# reverse proxy / ingress udenfor. 8080 er .NET 8's non-privilegerede default.
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Exercise-Api.dll"]
