## Context

The app uses a named `"Api"` HttpClient registered via `IHttpClientFactory` in `MauiProgram.cs`. All API calls go through `RequestProvider`, which creates requests from this client. Currently no `DelegatingHandler` exists in the pipeline and no default headers are set beyond per-request `Authorization` and occasional custom headers.

## Goals / Non-Goals

**Goals:**
- Every HTTP request to the backend includes a `User-Agent` header identifying the app
- The header includes app name, version, platform, and OS version
- Zero changes to `RequestProvider` or any service code — fully transparent

**Non-Goals:**
- Custom headers for non-API HttpClients (only the `"Api"` named client)
- Server-side parsing or validation of the User-Agent string
- Making the User-Agent format configurable at runtime

## Decisions

### 1. Use a DelegatingHandler, not default headers on HttpClient

**Choice**: Create a `UserAgentHandler : DelegatingHandler` that adds the `User-Agent` header in `SendAsync`.

**Why**: A `DelegatingHandler` participates in the `IHttpClientFactory` pipeline and is the standard .NET pattern for cross-cutting HTTP concerns. It's testable, composable, and keeps `MauiProgram.cs` configuration clean. Default headers on the HttpClient instance would also work but don't compose as well if more handlers are added later.

**Alternative considered**: Setting `client.DefaultRequestHeaders.UserAgent` in the `AddHttpClient` configuration lambda. Simpler but less extensible — a handler is the idiomatic approach and sets a precedent for future cross-cutting concerns (logging, retry, etc.).

### 2. User-Agent format

**Choice**: `SportowyHub/{appVersion} ({platform} {osVersion})`

Examples:
- `SportowyHub/1.0.0 (Android 14)`
- `SportowyHub/1.2.3 (iOS 17.4)`
- `SportowyHub/1.0.0 (WinUI 10.0.19041)`

**Why**: Follows the standard HTTP User-Agent convention (`ProductName/Version (Comment)`). Includes enough detail for the backend to identify the app, its version, and the platform without being overly verbose.

### 3. Device info from AppInfo and DeviceInfo

**Choice**: Use `Microsoft.Maui.ApplicationModel.AppInfo.Current.VersionString` for app version and `Microsoft.Maui.Devices.DeviceInfo.Current.Platform` + `DeviceInfo.Current.VersionString` for platform/OS.

**Why**: These are the standard MAUI Essentials abstractions — cross-platform, no native interop needed. Available on all target platforms.

### 4. Build User-Agent string once, cache it

**Choice**: Compute the User-Agent string in the handler's constructor and reuse it for every request.

**Why**: The app version and platform don't change during a session. Avoids repeated string concatenation on every HTTP call.

## Risks / Trade-offs

- **MAUI Essentials availability in handler constructor** → `AppInfo` and `DeviceInfo` require platform initialization. Since the handler is resolved from DI after `MauiApp.Build()`, this is safe. If construction timing changes, a lazy approach could be used instead.
- **User-Agent header already present on request** → The handler uses `TryAddWithoutValidation` to avoid overwriting if a caller explicitly sets one. Low risk since no current code sets `User-Agent`.
