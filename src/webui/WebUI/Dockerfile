# Dockerfile for hosted Blazor WASM
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj for caching
COPY src/webui/WebUI/WebUI.csproj ./WebUI.csproj
RUN dotnet restore "WebUI.csproj"

# Copy all source files
COPY src/webui/WebUI/ ./

# Build and publish
RUN dotnet publish "WebUI.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Run the ASP.NET Core host
ENTRYPOINT ["dotnet", "WebUI.dll"]