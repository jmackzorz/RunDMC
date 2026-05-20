# Project: Run DMC (Data Metrics Collector)

## Overview
Fitness tracking REST API that records workouts and surfaces analytical insights (weekly summaries, personal records, trend analysis). The goal is to demonstrate time-series data handling, aggregation queries, and basic analytics patterns in ASP.NET Core.

## Stack
- Language:       C# (.NET 10)
- Runtime:        .NET 10
- Framework:      ASP.NET Core Web API (minimal hosting model)
- Database:       SQL Server (LocalDB for development)
- ORM/Query:      Entity Framework Core 10 — LINQ for aggregation, no raw SQL
- Auth:           none
- Testing:        none yet
- Package mgr:    NuGet (dotnet CLI / Visual Studio)
- CI/CD:          none

## Project Structure
```
RunDMC/                     — solution root
  RunDMC/                   — solution folder (RunDMC.slnx)
    RunDMC/                 — project folder (RunDMC.csproj)
      Controllers/          — API controllers (UsersController, WorkoutsController, StatsController)
      Data/                 — EF Core DbContext (FitnessDbContext)
      DTOs/                 — request/response shapes
      Mappings/             — AutoMapper profiles (FitnessProfile)
      Models/               — domain entities (User, Workout, ActivityType)
      Services/             — business logic (WorkoutService, StatsService + interfaces)
      Program.cs            — app bootstrap / DI registration
      appsettings.json      — connection strings and logging config
```

## Architecture Decisions
- **Service layer over thin controllers** — controllers delegate all logic to `IWorkoutService` / `IStatsService`; controllers stay free of EF and business logic.
- **UsersController is an exception** — direct DbContext access kept intentionally simple; users are a thin resource with no business logic.
- **AutoMapper for DTO mapping** — `FitnessProfile` is the single mapping source of truth; no manual mapping in controllers or services.
- **LINQ-over-EF for aggregation** — weekly grouping is done in-process after a filtered DB fetch (not GroupBy-translated SQL) to keep the query simple and correct across EF versions.
- **Three targeted queries for personal records** — each PR category (distance, pace, duration) is a separate SQL query with `OrderBy` + `FirstOrDefault`, letting SQL Server do the sort rather than pulling all rows.
- **Trend analysis uses fixed 28/56-day windows** — recent = last 28 days vs. previous 28 days; stable threshold is ±2%.
- **No auth** — this is a demo/learning project; auth is explicitly out of scope.
- **ActivityType seeded in `OnModelCreating`** — Running, Cycling, Strength Training, Swimming, Walking are fixed seed data; do not add new ones without discussion.

## Coding Conventions
- Naming:         PascalCase for types and members, camelCase for locals; file names match class names
- Function style: Primary constructors preferred (`class Foo(DepA a, DepB b)`)
- Error handling: Return `null` from services when a resource is not found; controllers translate to `NotFound()`; no exceptions for control flow
- Imports:        Standard `using` at file top; no global usings beyond what the SDK provides implicitly
- Comments:       Only when the why is non-obvious (e.g., why three separate queries, why in-process grouping)
- Async:          All I/O methods are `async Task<T>`; suffix `Async` on all async methods

## Testing Expectations
No tests exist yet. When added:
- Unit test service logic (StatsService calculations) with an in-memory EF provider or mocked DbContext.
- Integration tests for controllers against a real LocalDB or SQL Server test database.
- Do not mock EF at the repository level — the project has no repository abstraction.

## Dependencies
```
Approved (already in use):
  AutoMapper 15.x             — DTO mapping
  EF Core 10 (SqlServer)      — ORM + migrations
  Microsoft.AspNetCore.OpenApi — OpenAPI/Swagger doc generation

Off-limits (do not add without discussion):
  MediatR / CQRS libraries    — overkill for this scope
  Dapper / raw SQL helpers     — EF is the settled choice
  Any auth middleware          — auth is out of scope
```

## Off-Limits Areas
- Do not modify `FitnessDbContext.OnModelCreating` seed data without discussion — changing seed data requires a new EF migration.
- Do not add EF migrations directly; ask first so the migration can be reviewed before it touches the DB schema.
- Do not change `appsettings.json` connection strings — local dev uses `(localdb)\mssqllocaldb`.

## Current Focus
Core API is feature-complete for the initial scope (workout CRUD, weekly stats, personal records, trend analysis). Next candidates per the README stretch goals: chart-ready endpoints, CSV export, SignalR live tracking — none started yet.

## Known Issues / Tech Debt
- `UsersController` accepts and returns the raw `User` entity instead of a DTO — serialization includes the `Workouts` navigation collection, which will be empty but is still exposed.
- Weekly aggregation is done in-process rather than translated to SQL GROUP BY; acceptable at current scale but will not scale to large datasets.
- No pagination on `GET /api/workouts/user/{userId}` — returns all workouts for a user unbounded.
- No input validation attributes on DTOs beyond EF-level constraints.
