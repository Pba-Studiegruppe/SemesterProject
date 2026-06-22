# syntax=docker/dockerfile:1

# =============================================================================
# Assign.Web.Teacher (Blazor Server)
# Targeter net10.0 (de øvrige projekter er net8.0) -> derfor egne SDK/runtime
# tags her. Projektet bruger Tailwind CSS, så vi har et separat Node-trin der
# kompilerer CSS'en, før .NET-publish kører.
# =============================================================================

# ---- CSS build stage ---------------------------------------------------------
# Tailwind kompileres med Node og kopieres ind i .NET-buildet. Dette holdes
# adskilt så Node IKKE ender i runtime-imaget.
FROM node:20-alpine AS css
WORKDIR /css

# package.json + lockfile først -> npm ci caches indtil dependencies ændres.
COPY ["Assign.Web.Teacher/package.json", "Assign.Web.Teacher/package-lock.json", "./"]
RUN npm ci

# Kopiér det Tailwind har brug for: config, postcss og selve kildefilerne
# (content-globben i tailwind.config.js scanner Components/Pages/wwwroot).
COPY ["Assign.Web.Teacher/tailwind.config.js", "Assign.Web.Teacher/postcss.config.js", "./"]
COPY ["Assign.Web.Teacher/", "./src/"]

# Byg den endelige minificerede stylesheet. Input-stien antager en
# wwwroot/css/app.css med @tailwind-direktiver. Justér --input hvis din
# kilde-CSS ligger et andet sted.
RUN npx tailwindcss \
      -c tailwind.config.js \
      -i ./src/wwwroot/css/app.css \
      -o /css/out/app.min.css \
      --minify

# ---- .NET build stage --------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Restore. Blazor-projektet har ingen ProjectReferences i den viste csproj,
# men hvis du tilføjer dem (fx Assign-Frontend), så tilføj deres csproj her.
COPY ["Assign.Web.Teacher/Assign.Web.Teacher.csproj", "Assign.Web.Teacher/"]
RUN dotnet restore "Assign.Web.Teacher/Assign.Web.Teacher.csproj"

COPY . .

# Hent den kompilerede CSS ind fra css-stagen, så publish samler den med.
COPY --from=css /css/out/app.min.css "Assign.Web.Teacher/wwwroot/css/app.min.css"

WORKDIR "/src/Assign.Web.Teacher"
RUN dotnet publish "Assign.Web.Teacher.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# ---- Runtime stage -----------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
USER $APP_UID
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Assign.Web.Teacher.dll"]
