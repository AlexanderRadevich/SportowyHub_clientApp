# Split Auth Service Interface

## Why

`IAuthService` has 14 members spanning login, registration, profile management, avatar upload, OAuth, and token management. This violates the Interface Segregation Principle -- consumers that only need a token must depend on the entire auth surface.

## What Changes

Split `IAuthService` into three focused interfaces: `ITokenProvider` for token access, `IAuthService` for authentication flows, and `IProfileService` for profile CRUD and avatar operations. The existing `AuthService` class implements all three initially.

## Capabilities

### New

- `ITokenProvider` interface (GetTokenAsync, RefreshTokenAsync)
- `IProfileService` interface (profile CRUD, avatar upload)

### Modified

- `IAuthService` slimmed to auth-only methods (login, register, logout, OAuth)
- `AuthService` implements all three interfaces
- DI registrations updated for all three interfaces
- Consumer injections narrowed to the interface they actually need

## Impact

- **Scope:** Auth layer, DI configuration, all ViewModels/services consuming auth
- **Risk:** Low -- additive split, existing class stays intact
- **Testing:** Mocks become simpler as consumers mock only what they use
- **UX:** No user-visible changes
