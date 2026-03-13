# Externalize API Config

## Why

`ApiConfig.cs` hardcodes `DefaultBaseUrl` to an ngrok dev tunnel and `GoogleClientId` in plain source. Fresh installs route all traffic through the dev tunnel. Release builds risk leaking user data to a temporary ngrok endpoint.

## What Changes

Move API base URL and Google Client ID to build-time configuration. Use conditional compilation or an `appsettings.json` equivalent per build configuration (Debug/Release). Ensure release builds never fall back to ngrok URLs.

## Capabilities

### New
- Environment-based configuration mechanism for build-specific settings
- Release build validation that fails if an ngrok URL is present

### Modified
- `ApiConfig.cs` — read config from external source instead of hardcoded constants
- `.csproj` — build properties for environment-specific configuration
- `MauiProgram.cs` — register configuration in DI

## Impact

- **Security** — eliminates data leak risk from ngrok URLs in production
- **Operations** — enables proper staging/production URL management
- **Development** — developers can override API URL without modifying tracked source
