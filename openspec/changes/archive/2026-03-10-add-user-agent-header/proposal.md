## Why

The backend has no way to identify which client is making API calls. Adding a `User-Agent` header to all HTTP requests lets the backend distinguish the MAUI mobile app from other consumers (web, scripts, tests), enabling better logging, analytics, rate limiting, and debugging.

## What Changes

- Add a `DelegatingHandler` that injects a `User-Agent` header into every outgoing HTTP request on the `"Api"` named client
- The User-Agent string SHALL include the app name, version, platform, and OS version (e.g., `SportowyHub/1.0.0 (Android 14; sdk 34)`)
- Register the handler in `MauiProgram.cs` on the existing `"Api"` HttpClient pipeline

## Capabilities

### New Capabilities

_(none)_

### Modified Capabilities

- `http-client-infrastructure`: Add a requirement for a `User-Agent` header on all API requests via a `DelegatingHandler`

## Impact

- **MauiProgram.cs** — Register the new `DelegatingHandler` on the `"Api"` client
- **New file** — `DelegatingHandler` implementation in `Services/Api/`
- **No breaking changes** — transparent to all existing service code
- **No new dependencies** — uses built-in `Microsoft.Maui.Essentials` for device info
