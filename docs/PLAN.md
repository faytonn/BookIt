# BookIt completion plan

One numbered item = one commit. Tick items as they land. Audit numbers in parentheses refer to the Phase 3 audit (2026-10-23).

Decisions: RabbitMQ added with one real use (SeatSold → confirmation email); Testcontainers runs locally (Docker Desktop) and in CI; Caddy serves plain HTTP on the VM IP (no domain); order M1 → M2 → M3 → M4 → …

## M1 — Critical/high fixes

- [x] 1. Secrets out of `appsettings.json` → user secrets / env vars; `.env.example`; real `.gitignore`; untrack `bin/`, `obj/`, `*.user`. (3, 21)
- [x] 2. `[Authorize(Roles = "Admin")]` on all Admin controllers; `[Authorize]` on `Account/Settings`; admin redirect → `/Admin/Dashboard/Index`. (1, 8)
- [x] 3. `ReservationService`: missing `SaveChangesAsync`; include `SeatType` when pricing; link seats via navigation; single save. (2, 5, 6)
- [ ] 4. `Reservation/Create.cshtml`: plain form post + validation summary. (7)
- [ ] 5. Keep existing event image when no new file uploaded. (11)
- [ ] 6. Localization: align ISO codes (`en`/`az`/`cs`) in seed, cultures, `CookieService`; add `Home/SetLanguage`; migration. (9)
- [ ] 7. Missing Account views: `ForgotPassword`, `ResetPassword`, `ResetPasswordConfirmation`, `ResetPasswordSuccess`, `EmailVerificationFailure`. (10)
- [ ] 8. Delete EventComposite controller/service/DTOs/mapper/repo interface. (12)
- [ ] 9. Login hardening: drop `CheckPasswordAsync` pre-check, enable lockout, explicit resend-confirmation action; hardcode `Role.User` at registration; check `IdentityResult`. (13, 14)
- [ ] 10. Replace 2.3.0 `Microsoft.AspNetCore.*` package refs with `FrameworkReference`; bump AutoMapper, MailKit; align Localization.Abstractions to 8.x. (19)

## M2 — Medium fixes

- [ ] 11. Wire `.WithMany(u => u.Reservations)` / `.WithMany(e => e.WaitlistEntries)`; migration drops shadow `ApplicationUserId`, `EventId1`. (15)
- [ ] 12. `ReservationSeatConfiguration` (`decimal(18,2)`, FK indexes). (4)
- [ ] 13. Matching query filters for `Seat`, `Reservation`, `CategoryDetail`, `NewsDetail`, `NotificationDetail`; remove duplicate `Review` filter. (16)
- [ ] 14. Hall: plain `Name` on Create/Update DTOs + views; delete empty `HallDetail` (migration). (17)
- [ ] 15. `Resources/ValidationMessages.{en,az,cs}.resx`; fix keys in `CustomIdentityErrorDescriber`. (18)
- [ ] 16. File upload validation in `EventController` + `CloudinaryService`. (20)
- [ ] 17. Remove sync-over-async (`ReservationService.GetAll`, `EventDetailController.Create`, `EmailService`); delete `IdentityRepository`. (22)
- [ ] 18. Reservation queries: single query with includes. (23)
- [ ] 19. `SeatService`: include `SeatType`; `BulkCreate` loads existing seats once. (24, 36)
- [ ] 20. `CategoryService`: fix uniqueness check. (25)
- [ ] 21. Deterministic seed dates; fix seeded email values. (26, 38)
- [ ] 22. `CategoryController.Delete` → POST + antiforgery; `SeatController.Delete` antiforgery; `Category/Index.cshtml` forms. (30)

## M3 — Public site

- [ ] 23. Public `EventController` (`Index` with filters, `Details`) + Boleto views; wire `Home/Index` search form.
- [ ] 24. `Account/Settings`: profile edit + change password.
- [ ] 25. News on home page.
- [ ] 26. Seat-map + booking page on `Event/Details` (placeholder until API).

## M4 — Seat availability redesign

- [ ] 27. `EventSeat` entity (`EventId`, `SeatId`, `Status`, `HeldUntil`, `HeldByUserId?`, `RowVersion`), config with unique `(EventId, SeatId)`, DbSet, repo.
- [ ] 28. Migration: create table, backfill per (event, seat in hall), drop `Seat.IsReserved`.
- [ ] 29. `EventSeatService`: `HoldAsync` / `ConfirmAsync` / `ReleaseAsync`; `DbUpdateConcurrencyException` → Conflict.
- [ ] 30. `HoldExpiryService : BackgroundService`, every minute.
- [ ] 31. Admin reservation flow + `EventController.Create` use `EventSeat`.
- [ ] 32. `BookIt.Tests` (xUnit + Testcontainers.MsSql): parallel-hold test + rule tests.

## M5 — API + health

- [ ] 33. `GET /api/events/{id}/seats`, `POST /api/events/{id}/holds` (201/409), `POST /api/holds/{id}/confirm`.
- [ ] 34. Health checks `/health/live`, `/health/ready` (SQL Server, RabbitMQ).
- [ ] 34b. RabbitMQ: publish `SeatSold` on confirm; hosted consumer sends confirmation email.
- [ ] 35. `AddRateLimiter` fixed-window per-IP on holds → 429.
- [ ] 36. `seed-loadtest` command: 1 hall, 5,000 seats, 1 event.
- [ ] 37. Booking page uses the API.

## M6 — Containers + CI

- [ ] 38. Multi-stage `Dockerfile`, non-root, `HEALTHCHECK`, `.dockerignore`.
- [ ] 39. `docker-compose.yml`: app + SQL Server + RabbitMQ, healthchecks, `migrator` one-off service.
- [ ] 40. `.github/workflows/ci.yml`: build/test, gitleaks, docker build, Trivy, push to `ghcr.io` on main; least-privilege `permissions`, `concurrency`.
- [ ] 41. README badge + quick-start.

## M7 — Infra

- [ ] 42. `infra/terraform/`: e2-medium Debian 12, `europe-west3`, static IP, firewall, variables, outputs.
- [ ] 43. `infra/ansible/`: roles `base`, `docker`, `deploy`, `caddy`; idempotent.
- [ ] 44. `.github/workflows/deploy.yml` (`workflow_dispatch`).

## M8 — Observability

- [ ] 45. OpenTelemetry instrumentation + custom metrics.
- [ ] 46. `otel-collector` in compose → Grafana Cloud.
- [ ] 47. Structured JSON logs with trace IDs.
- [ ] 48. Dashboard JSON, `SLO.md`, alert rules.

## M9 — Load test

- [ ] 49. `loadtest/onsale.js` + `verify-no-double-sell.sql`.
- [ ] 50. Run, find bottleneck, fix, re-run; record real numbers.

## M10 — Docs + polish

- [ ] 51. README.
- [ ] 52. `docs/postmortem-onsale-test.md`, `docs/runbook.md`.
- [ ] 53. Dead code sweep. (28, 29, 31–35, 39)
- [ ] 54. `GetPagesAsync` in admin index pages. (37)
