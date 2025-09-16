# Repository Guidelines

## Project Structure & Modules
- Solution: `KafkaSolution.sln` at repo root.
- Services (CQRS pattern):
  - Command side: `Sm-Post/Post.Cmd/*` (`Api`, `Domain`, `Infrastructure`).
  - Query side: `Sm-Post/Post.Query/*` (`Api`, `Domain`, `Infrastructure`).
- Shared core (CQRS abstractions): `CORS-ES/CQRS.Core`.
- Docker Kafka stack: `docker-compose.yml` (Zookeeper + Kafka).

## Build, Run, and Kafka
- Restore and build:
  - `dotnet restore KafkaSolution.sln`
  - `dotnet build -c Debug KafkaSolution.sln`
- Run APIs locally (separate shells):
  - Command API: `dotnet run --project "Sm-Post/Post.Cmd/Post.Cmd.Api"`
  - Query API: `dotnet run --project "Sm-Post/Post.Query/Post.Query.Api"`
- Start Kafka (requires an external network):
  - `docker network create mydockernetwork` (one‑time)
  - `docker compose up -d`

## Coding Style & Naming
- Language: C# (.NET 8). Indentation: 4 spaces; UTF‑8; LF/CRLF per OS.
- Naming: PascalCase for types/methods/properties; camelCase for locals/params.
- Files and namespaces should mirror folder layout (e.g., `Post.Cmd.Domain` for code under `Post.Cmd/ Post.Cmd.Domain`).
- Prefer small, focused classes; keep API, Domain, Infrastructure boundaries clear.
- Formatting: run `dotnet format` before pushing.

## Testing Guidelines
- Current state: no test projects present. Add tests under `tests/` using xUnit.
- Naming: `<Project>.Tests` (e.g., `Post.Cmd.Domain.Tests`).
- Example setup:
  - `dotnet new xunit -n Post.Cmd.Domain.Tests -o tests/Post.Cmd.Domain.Tests`
  - `dotnet sln add tests/Post.Cmd.Domain.Tests`
  - Run tests: `dotnet test`

## Commit & Pull Requests
- Commits: concise, imperative subject; include scope when useful.
  - Examples: `feat(cmd-api): add create-post endpoint`, `fix(query-infra): handle null payload`.
- PRs must include:
  - Clear description, motivation, and screenshots or sample requests where relevant.
  - Linked issue (e.g., `Closes #123`).
  - Steps to validate (build/run commands, curl/Postman examples).

## Security & Configuration
- Do not commit secrets. Use environment variables or `dotnet user-secrets` for local dev.
- Kafka broker config is in `docker-compose.yml`; default advertised listener is `PLAINTEXT://localhost:9092`.
- App settings: prefer `appsettings.Development.json` overrides; document any required env vars in the PR.

