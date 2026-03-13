# Split Auth Service Interface -- Design

## Context

`IAuthService` is a 14-member interface that forces consumers to depend on capabilities they do not use. Services needing only a token (e.g., HTTP handlers) pull in profile and OAuth dependencies.

## Goals / Non-Goals

### Goals

- Each interface has a single, cohesive responsibility
- Consumers inject only the interface they need
- No behavioral changes -- pure structural refactor

### Non-Goals

- Splitting the implementation class into separate classes (can be done later)
- Changing auth flow logic or token storage

## Decisions

- `ITokenProvider`: `GetTokenAsync`, `RefreshTokenAsync` -- used by HTTP handlers and any service needing auth headers
- `IAuthService`: `LoginAsync`, `RegisterAsync`, `LogoutAsync`, OAuth methods -- used by auth ViewModels
- `IProfileService`: `GetProfileAsync`, `UpdateProfileAsync`, `UploadAvatarAsync` -- used by profile ViewModels
- `AuthService` implements all three; registered in DI as all three interface types pointing to one singleton
- Consumers updated to inject the narrowest interface

## Risks / Trade-offs

- **Three registrations for one class:** Acceptable; DI containers handle this cleanly
- **Merge conflicts:** Changes touching `IAuthService` consumers will conflict
- **Future split:** Implementation can be split into separate classes later without changing consumers
