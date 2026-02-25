## Context

The backend API has been updated. The OpenAPI spec reveals three categories of drift from the current client code:

1. **New `locale` field** in every JSON response (2xx and 4xx/5xx) — currently absent from all client models.
2. **New `ValidationError` format** for 422 responses — extends the existing error structure with a `violations` object. Currently, 422 errors are parsed as `ApiError`, losing field-level violation data.
3. **Password policy change** — API now enforces `minLength: 8`, but client validates at 6.

Current error flow: `RequestProvider.HandleResponse` throws `ServiceAuthenticationException` for 401/403, `HttpRequestException` for everything else (including 422). `AuthService` catches these and parses the JSON body to extract messages.

## Goals / Non-Goals

**Goals:**
- Align all auth response/error models with the current API contract
- Parse 422 `violations` into `AuthResult.FieldErrors` for both login and register
- Update client-side password validation to match the API's `minLength: 8`
- Adjust password strength indicator thresholds accordingly

**Non-Goals:**
- Using the `locale` field for app locale switching (the app has its own locale management via `Accept-Language`)
- Changing the `RequestProvider` error flow (it works correctly — 422 already throws `HttpRequestException` with the response body)
- Adding new registration form fields (the request schema is unchanged: email, password, optional phone)

## Decisions

### 1. Extend `ErrorDetail` rather than creating a separate `ValidationError` model

The API's `ValidationError` schema only differs from `ApiError` by having an extra `violations` field in the `error` object. Rather than maintaining two error models with two parsing paths, add an optional `Violations` property to `ErrorDetail`.

**Why**: One model, one deserialization path, less code. `JsonSerializer` with `PropertyNameCaseInsensitive` will simply leave `Violations` as null when the field is absent in regular `ApiError` responses.

**Alternative considered**: Separate `ValidationError` record — rejected because it adds a second parse attempt in every error handler, and the shape is nearly identical.

### 2. Add `Locale` property to `ApiError`, `LoginResponse`, and `RegisterResponse`

The API documentation states: _"All JSON responses (2xx and 4xx/5xx) include a top-level `locale` field."_ Each response model gets a `string? Locale` property. Since we use `SnakeCaseLower` naming policy, `locale` maps directly.

**Why not a base response class**: The models are positional records. Adding inheritance complicates the positional constructor pattern and is over-engineered for a single string field.

### 3. `Violations` typed as `Dictionary<string, string>?`

The OpenAPI spec defines `violations` as `type: object`. This matches the existing `AuthResult.FieldErrors` type (`Dictionary<string, string>?`), keeping the mapping straightforward: parse violations → pass directly as field errors.

**If the actual format differs** (e.g., arrays of messages per field), we adjust the type and add a flattening step. Starting simple.

### 4. Password thresholds: min 8, strength bands shift up

- `CanCreateAccount()`: change `Password.Length < 6` → `Password.Length < 8`
- Strength indicator:
  - **Weak**: < 8 characters (below API minimum — user can't submit)
  - **Medium**: 8+ chars with letters and digits (meets API minimum)
  - **Strong**: 10+ chars with letters, digits, and special characters (shifted from 8 to 10 for meaningful differentiation above the new minimum)

### 5. Login 422 handling uses the same `ParseErrorWithFields` path

Currently `LoginAsync` uses `ParseErrorMessage` (message-only). Switch to `ParseErrorWithFields` so that if the server returns 422 with violations on login, they can be surfaced. The `LoginViewModel` already has `EmailError` — it can display field-level errors the same way `RegisterViewModel` does.

## Risks / Trade-offs

- **`violations` object shape is unverified** — the OpenAPI spec says `type: object` without detailing the value type. If the server returns arrays or nested objects, `Dictionary<string, string>` deserialization will fail silently (null values). → Mitigation: the fallback to `ErrorMessage` still works; field errors just won't populate. Easy to fix once we see real 422 responses.
- **Password strength "Strong" threshold raised to 10** — users with 8-9 char complex passwords will now see "Medium" instead of "Strong". → Mitigation: purely cosmetic indicator, doesn't block submission. Aligns with good UX (if 8 is the minimum, "Strong" should be meaningfully above it).
- **`locale` field stored but unused** — adds a property that no code reads. → Acceptable: it prevents deserialization warnings, documents the API contract, and is available if we need locale-aware error display later.
