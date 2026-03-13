# Design: Externalize API Config

## Context

`ApiConfig.cs` hardcodes the API base URL to an ngrok dev tunnel and the Google Client ID. These values are committed to source control and ship in release builds, creating a data leak risk.

## Goals / Non-Goals

### Goals
- Remove hardcoded ngrok URL and Google Client ID from source
- Provide per-environment configuration (Debug/Release)
- Fail the release build if an ngrok URL is detected

### Non-Goals
- Implement a full configuration management system
- Support runtime configuration changes
- Add remote config / feature flags

## Decisions

1. **MSBuild properties approach** — Define `ApiBaseUrl` and `GoogleClientId` as MSBuild properties in `.csproj` with condition on `$(Configuration)`. These get embedded as constants via `<DefineConstants>` or generated source.
2. **Separate config files** — Use `appsettings.Debug.json` and `appsettings.Release.json` as embedded resources, loaded at startup. This is more flexible than MSBuild properties and follows .NET conventions.
3. **Build-time validation** — Add an MSBuild target that runs on Release configuration and fails if the resolved base URL contains "ngrok".
4. **Developer override** — Support a `SPORTOWYHUB_API_URL` environment variable or local file for developer-specific overrides without modifying tracked files.

## Risks / Trade-offs

- **MAUI resource loading** — Embedded JSON resources require platform-aware loading. Must test on all target platforms.
- **Migration effort** — All code referencing `ApiConfig.DefaultBaseUrl` must be updated to use the new config source.
- **Secret rotation** — Google Client ID is still in a config file (though not hardcoded in C#). For true secret management, would need a build secret injection step.
