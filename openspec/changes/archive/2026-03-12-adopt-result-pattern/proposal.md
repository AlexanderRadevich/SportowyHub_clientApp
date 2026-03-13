# Adopt Result Pattern Across Services

## Why

`AuthService` uses `AuthResult<T>` but all other services let exceptions propagate raw. The CLAUDE.md architecture spec calls for a `Result<T>` pattern for service-layer error propagation. ViewModels currently need try/catch around every service call, creating inconsistent error handling and cluttered command methods.

## What Changes

Create a generic `Result<T>` type and apply it across all service interfaces. ViewModels will handle `Result` discriminated returns instead of catching exceptions, producing cleaner control flow and consistent error reporting.

## Capabilities

### New

- Generic `Result<T>` type with `Success` and `Failure` factory methods
- Consistent service return type across the entire service layer

### Modified

- All service interfaces return `Result<T>` instead of raw `T`
- All service implementations wrap API calls in `Result<T>`
- All ViewModels switch from try/catch to `Result` pattern matching
- `AuthResult<T>` retired in favor of unified `Result<T>`

## Impact

- **Scope:** All services, all ViewModels that consume services
- **Risk:** Medium -- wide refactor, but mechanical in nature
- **Testing:** Existing tests need updated return types; new tests for Result type itself
- **UX:** No user-visible changes; error messages remain the same
