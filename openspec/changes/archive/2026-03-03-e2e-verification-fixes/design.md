## Context

After implementing the `sync-api-endpoints` change, E2E verification against the running backend revealed 4 model mismatches in the profile update flow. The search and favorites-remove integrations are correct and require no code changes. All fixes are confined to the profile-related models and the auth service layer.

## Goals / Non-Goals

**Goals:**
- Fix all profile model mismatches so PATCH `/api/private/profile` works correctly end-to-end
- Ensure GET `/api/private/profile` deserializes all fields accurately
- Maintain backward compatibility with existing `EditProfileViewModel` usage

**Non-Goals:**
- Changing the backend API contract (fixes are client-side only)
- Adding new profile editing features or UI changes
- Fixing search or favorites (verified correct)

## Decisions

**Decision 1: Make `NotificationsEnabled` nullable (`bool?`) in `UpdateProfileAccountRequest`**
- Rationale: The backend treats `null`/absent `notifications_enabled` as no-op. A non-nullable `bool` always serializes a value, preventing true partial PATCH semantics. Making it `bool?` allows omission when the field hasn't changed.
- Alternative: Add `[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]` — rejected because `false` is a valid value that would then be incorrectly omitted.

**Decision 2: Remove top-level `Locale` from `UserProfile`**
- Rationale: The backend GET response nests `locale` inside `account`, not at the root level. The top-level `UserProfile.Locale` always deserializes as `null`. Removing it prevents confusion — locale is accessible via `Account.Locale`.
- Alternative: Keep it and populate from `Account.Locale` via a custom constructor — rejected as unnecessary indirection.

**Decision 3: Create a dedicated `UpdateProfileResponse` record**
- Rationale: The PATCH response returns only `{ "account": {...} }`, not a full `UserProfile`. Deserializing into `UserProfile` produces a broken object with zeroed/null fields for `Id`, `Email`, etc. A dedicated response type accurately represents the partial response.
- Alternative: Ignore the response entirely (currently discarded) — rejected because future consumers would hit the same bug.

**Decision 4: Change `ReputationScore` to `decimal` with `FlexibleDecimalConverter`**
- Rationale: The backend method is named `getReputationScoreDecimal()`, implying a fractional value. Using `int` would throw a `JsonException` if the backend ever returns `4.75`. Reusing `FlexibleDecimalConverter` handles the string-vs-number inconsistency pattern already seen with `price`.
- Alternative: Use `double` — rejected because `decimal` is preferred for precise values and matches the converter we already have.

## Risks / Trade-offs

- [Risk] Removing `UserProfile.Locale` breaks any code that reads `profile.Locale` directly → Mitigation: Grep confirms no code reads `UserProfile.Locale` — all locale access goes through `Account.Locale`.
- [Risk] Changing `NotificationsEnabled` to `bool?` requires callers to explicitly set it → Mitigation: `EditProfileViewModel` already reads the current value from the profile and passes it through — just change to `bool?` type.
- [Trade-off] `UpdateProfileResponse` is a new model for a response that's currently discarded → Acceptable cost for correctness; prevents future bugs if the return value is ever consumed.
