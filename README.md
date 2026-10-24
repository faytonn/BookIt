# BookIt

[![CI](https://github.com/faytonn/BookIt/actions/workflows/ci.yml/badge.svg)](https://github.com/faytonn/BookIt/actions/workflows/ci.yml)

Event ticketing site with a concurrency-safe seat booking flow: pick a seat, hold it for 10 minutes, confirm. Holds and sales are protected by a SQL Server rowversion token, so two buyers can never get the same seat.

**Stack:** ASP.NET Core 8 MVC, EF Core 8 + SQL Server, ASP.NET Identity, RabbitMQ (seat-sold events → confirmation email), Cloudinary, MailKit. Admin panel for events, venues, seat maps and reservations.

## Quick start (Docker)

```bash
cp .env.example .env        # fill in SMTP / Cloudinary / admin password
docker compose up -d --build
```

Then open <http://localhost:8080> (change `APP_PORT` in `.env` if 8080 is taken).
Compose starts SQL Server and RabbitMQ, runs the one-off `migrator` service (migrations + seed), then the app.

```bash
docker compose run --rm app seed-loadtest   # optional: 5,000-seat event + 100 test users
docker compose logs -f app
docker compose down                          # add -v to drop the database volume
```

## Quick start (local .NET)

Needs .NET 8 SDK and a SQL Server instance (RabbitMQ optional: leave `RabbitMq:Host` unset to disable messaging).

```bash
dotnet user-secrets set ConnectionStrings:DefaultConnection "Server=.;Database=BookItDB;Trusted_Connection=true;TrustServerCertificate=true" -p BookIt.Presentation
dotnet user-secrets set Admin:Email you@example.com -p BookIt.Presentation
dotnet user-secrets set Admin:Password "ChangeMe_Str0ng!" -p BookIt.Presentation
cd BookIt.Presentation && dotnet run --launch-profile http     # http://localhost:5078
```

Migrations and the admin user are applied on startup.

## Tests

```bash
dotnet test BookIt.sln      # needs Docker: Testcontainers starts a real SQL Server
```

## Endpoints

| Path | What |
|---|---|
| `GET /health/live` | process is up |
| `GET /health/ready` | SQL Server and RabbitMQ reachable |
| `GET /api/events/{id}/seats` | seat map with live status |
| `POST /api/events/{id}/holds` | hold a seat (`{"seatId": n}`, rate-limited per IP) |
| `POST /api/holds/{id}/confirm` | buy the held seat |
| `DELETE /api/holds/{id}` | release a hold |

Write endpoints need a signed-in user and an `X-Requested-With` header.

## CI

Every push and pull request to `master` runs `.github/workflows/ci.yml`: build + tests, gitleaks secret scan, Docker build, Trivy image scan. Pushes to `master` also publish the image to `ghcr.io/faytonn/bookit` tagged `latest` and `sha-<commit>`.

Project checklist and roadmap: [`docs/PLAN.md`](docs/PLAN.md).
