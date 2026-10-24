# syntax=docker/dockerfile:1

# ---- build stage: SDK image, restore first so NuGet is cached between source edits ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY BookIt.Domain/BookIt.Domain.csproj BookIt.Domain/
COPY BookIt.Application/BookIt.Application.csproj BookIt.Application/
COPY BookIt.Persistence/BookIt.Persistence.csproj BookIt.Persistence/
COPY BookIt.Infrastracture/BookIt.Infrastracture.csproj BookIt.Infrastracture/
COPY BookIt.Presentation/BookIt.Presentation.csproj BookIt.Presentation/
RUN dotnet restore BookIt.Presentation/BookIt.Presentation.csproj

COPY . .
RUN dotnet publish BookIt.Presentation/BookIt.Presentation.csproj \
    -c Release -o /app/publish --no-restore

# ---- runtime stage: ASP.NET runtime only, no SDK, non-root ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# curl is only for the container HEALTHCHECK
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

# data-protection keys (auth cookies) live here; compose mounts a volume so logins survive restarts
RUN mkdir -p /home/app/.aspnet/DataProtection-Keys && chown -R app:app /home/app/.aspnet

# `app` (uid 1654) ships with the aspnet image; port 8080 needs no root
USER app
ENV ASPNETCORE_HTTP_PORTS=8080 \
    DOTNET_RUNNING_IN_CONTAINER=true
EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=5s --start-period=40s --retries=3 \
    CMD curl -fsS http://localhost:8080/health/live || exit 1

ENTRYPOINT ["dotnet", "BookIt.Presentation.dll"]
