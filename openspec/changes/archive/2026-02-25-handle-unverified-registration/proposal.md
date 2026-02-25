## Why

The registration endpoint can return a response without requiring email verification (trust level `TL0`). Currently, the app always navigates to the email verification page after successful registration. When the backend skips verification, the user gets stuck on a verification screen waiting for an email that will never arrive.

## What Changes

- Detect when the registration response indicates no verification is needed (user registered directly at `TL0`)
- Show a success message ("Account created successfully") and navigate to the login page instead of the email verification page
- Add a localized success message for unverified registration

## Capabilities

### New Capabilities

_(none)_

### Modified Capabilities

- `auth-screens`: Add conditional post-registration navigation — login page (unverified/TL0) vs email verification page (verification required)

## Impact

- `RegisterViewModel.cs` — branch navigation logic based on `TrustLevel` from `RegisterResponse`
- Localization `.resx` files (pl, en, uk, ru) — new success message string
- No API or model changes needed — `RegisterResponse` already contains `TrustLevel`
