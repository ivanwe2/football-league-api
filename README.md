# Football League REST API

![.NET](https://github.com/ivanwe2/football-league-api/actions/workflows/dotnet.yml/badge.svg)

A .NET 8 Web API for managing football teams, matches, and automatic league rankings. This solution demonstrates **Clean Architecture**, **some DDD principles**, and **Enterprise-grade patterns**.

> **Note:** The application includes a Feature Flag (`EnableSeeding`). If enabled in `appsettings.json`, it will automatically seed the database with English Premier League teams and random match results on startup.

---

## 🏗 Architecture & Patterns

The solution follows a strict **N-Tier Architecture** with **Rich Domain Models**.

### Project Structure
* **`FootballLeague.Domain`**: The core business logic. Contains Entities, Value Objects, and Domain Logic.
* **`FootballLeague.Data`**: Infrastructure layer. Contains EF Core configurations, Repositories, and Interceptors.
* **`FootballLeague.Services`**: Business Logic layer. Orchestrates data flow and business rules using Strategies.
* **`FootballLeague.API`**: The entry point. Contains Minimal API endpoints, Exception Handling, and Migration logic.

### Design Patterns Implemented
* **Repository Pattern:** Specific repositories (`TeamRepository`, `MatchRepository`) to encapsulate data access and optimize queries (e.g., eager loading via `Include`).
* **Strategy Pattern:** Used for `ScoringStrategy` to encapsulate the logic for calculating (and reverting) points for Wins/Draws/Losses.
* **Result Pattern:** Replaces Exceptions for control flow. Methods return `Result<T>` (Success/Failure) for predictable handling.
* **Factory Pattern:** Static factory methods (e.g., `Team.Create`) ensure entities are never instantiated in an invalid state.
* **Decorator/Interceptor:** An EF Core `AuditingInterceptor` automatically tracks `CreatedOn`, `LastModifiedOn`, and `SoftDelete` properties without polluting business logic.

---

## 🛠 Tech Stack

* **Framework:** .NET 8 (C#)
* **Database:** Microsoft SQL Server (2022)
* **ORM:** Entity Framework Core (Code First)
* **Testing:** xUnit & Moq
* **Documentation:** Swagger / OpenAPI
* **Other:** Feature Management (Feature Flags)

---

## 🧪 Running Tests

The solution includes Unit Tests for the Service Layer
