# ThreatLens

ThreatLens is an ASP.NET Core Web API that is meant to collect security logs and, over time, detect threats from them (for example repeated failed logins from the same IP).

The project is at an early stage. Only the parts listed under "What works now" exist in the code today.

## What works now

- `GET /hello` returns a status message and the API version (`0.1`).
- `SecurityLog` model with `IpAddress`, `TimeStamp`, `EventType`, `UserName` and `Status`.
- `GET /api/logs` returns the stored security logs (two sample logs are seeded at startup).
- `POST /api/logs` adds a new security log and returns `201 Created`.
- Swagger UI and OpenAPI document in the Development environment.

Logs are kept in an in-memory list, so they are lost when the API stops. There is no database, authentication or threat detection yet.

## Tech stack

- C#
- .NET 10
- ASP.NET Core (Minimal API)
- Swashbuckle (Swagger UI), Microsoft.AspNetCore.OpenApi

## Running

Requires the .NET 10 SDK.

```bash
cd ThreatLens.Api
dotnet run
```

The API listens on `http://localhost:5183` (http profile). Swagger UI: `http://localhost:5183/swagger`.

Example request:

```bash
curl -X POST http://localhost:5183/api/logs \
  -H "Content-Type: application/json" \
  -d '{"ipAddress":"192.168.1.20","timeStamp":"2026-11-06T10:00:00Z","eventType":"LOGIN_FAILED","userName":"admin","status":"Failed"}'
```

## Planned

Not implemented yet:

- PostgreSQL for persistent log storage
- Redis for caching and rate tracking
- RabbitMQ for asynchronous log ingestion
- Docker setup
- xUnit tests
- Threat detection rules (e.g. brute-force login detection)
