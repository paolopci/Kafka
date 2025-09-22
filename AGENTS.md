# Repository Guidelines

## Project Structure & Modules
- Solution: `KafkaSolution.sln` at repo root.
- Services (CQRS pattern):
  - Command side: `Sm-Post/Post.Cmd/*` (`Api`, `Domain`, `Infrastructure`).
  - Query side: `Sm-Post/Post.Query/*` (`Api`, `Domain`, `Infrastructure`).
- Shared core (CQRS abstractions): `CORS-ES/CQRS.Core`.
- Docker Kafka stack: `docker-compose.yml` (Zookeeper + Kafka).

## Scope & Agent Notes
- Scope: This AGENTS.md applies to the entire repository.
- Be surgical: change only what’s required; preserve existing names/paths.
- Keep CQRS boundaries intact: do not mix Command/Query responsibilities.
- Prefer minimal, focused diffs and align namespaces to folders.
- Do not commit secrets or alter `docker-compose.yml` unless explicitly requested.

## Shared Library (Post.Common)
- Path: `Post.Common/Post.Common.csproj` with namespace root `Post.Common`.
- Purpose: cross-cutting DTOs, contracts, helpers used by Command/Query sides.
- Referencing rules:
  - `Post.Cmd.Api`, `Post.Cmd.Domain`, `Post.Cmd.Infrastructure` should reference `Post.Common` and `CORS-ES/CQRS.Core` as needed.
  - Keep domain models independent; avoid circular references.
- Legacy note: the old `Post.Common/Post.Cmd.Domain/ClassLibrary1` folder is deprecated. Do not reference it; remove after migration when safe.

## Build, Run, and Kafka
- Restore and build:
  - `dotnet restore KafkaSolution.sln`
  - `dotnet build -c Debug KafkaSolution.sln`
- Run APIs locally (separate shells):
  - Command API: `dotnet run --project "Sm-Post/Post.Cmd/Post.Cmd.Api"`
  - Query API: `dotnet run --project "Sm-Post/Post.Query/Post.Query.Api"`
- Start Kafka (requires an external network):
  - `docker network create mydockernetwork` (one-time)
  - `docker compose up -d`

### Tips
- Ensure Docker Desktop and the Compose plugin are installed and running.
- Kafka broker advertises `PLAINTEXT://localhost:9092`. Keep APIs configured accordingly.
- If you add new services, wire configs via `appsettings.Development.json` or env vars.

## Coding Style & Naming
- Language: C# (.NET 8).
  - Indentation: 4 spaces.
  - Encoding: UTF-8; line endings per OS (LF on *nix, CRLF on Windows).
- Naming: PascalCase for types/methods/properties; camelCase for locals/params.
- Files and namespaces should mirror folder layout (e.g., `Post.Cmd.Domain` for code under `Post.Cmd/Post.Cmd.Domain`).
- Prefer small, focused classes; keep API, Domain, Infrastructure boundaries clear.
- Formatting: run `dotnet format` before pushing.

### CQRS Boundaries
- Command side (`Sm-Post/Post.Cmd/*`): write models, commands, command handlers, and Kafka producers.
- Query side (`Sm-Post/Post.Query/*`): read models, queries, query handlers, and any projections/consumers.
- Shared abstractions live in `CORS-ES/CQRS.Core`. Avoid runtime coupling between Cmd and Query.

## Testing Guidelines
- Current state: no test projects present. Add tests under `tests/` using xUnit.
- Naming: `<Project>.Tests` (e.g., `Post.Cmd.Domain.Tests`).
- Example setup:
  - `dotnet new xunit -n Post.Cmd.Domain.Tests -o tests/Post.Cmd.Domain.Tests`
  - `dotnet sln add tests/Post.Cmd.Domain.Tests`
  - Run tests: `dotnet test`

### Test Scope
- Unit tests for Domain and Infrastructure are preferred; keep API tests light.
- Avoid spinning up Kafka in unit tests; abstract producers/consumers behind interfaces and mock them.
- If adding integration tests later, gate them behind a category and exclude by default in CI.

## Commit & Pull Requests
- Commits: concise, imperative subject; include scope when useful.
  - Examples: `feat(cmd-api): add create-post endpoint`, `fix(query-infra): handle null payload`.
- PRs must include:
  - Clear description, motivation, and screenshots or sample requests where relevant.
  - Linked issue (e.g., `Closes #123`).
  - Steps to validate (build/run commands, curl/Postman examples).

### PR Checklist
- `dotnet build` succeeds; `dotnet format` produces no changes.
- New projects and tests are added to `KafkaSolution.sln`.
- API breaking changes are documented in the PR description.
- Configuration changes are reflected in `appsettings.Development.json` and documented.
 - Solution folders: place shared code under the `Post.Common` solution folder.

## Security & Configuration
- Do not commit secrets. Use environment variables or `dotnet user-secrets` for local dev.
- Kafka broker config is in `docker-compose.yml`; default advertised listener is `PLAINTEXT://localhost:9092`.
- App settings: prefer `appsettings.Development.json` overrides; document any required env vars in the PR.

## Troubleshooting
- Kafka connection refused: ensure docker network exists and `docker compose up -d` is running.
- Port conflicts: stop local apps on `9092` or adjust compose and configs together in a single PR.
- Schema/contract drift: if message contracts change, bump version and update both producers and consumers.

## Contributing Conventions
- Do not rename folders/namespaces casually; keep folder-to-namespace mapping strict.
- Keep Command and Query projects self-contained. Put cross-cutting code in `CORS-ES/CQRS.Core`.
- Avoid heavy frameworks for DI/logging beyond defaults unless asked; prefer .NET built-ins.
- Keep diffs small and explain rationale when refactoring.
