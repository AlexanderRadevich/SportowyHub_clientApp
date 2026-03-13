## Why

`RegisterViewModel.cs:194` accesses `registerResult.Data!.TrustLevel` using the null-forgiving operator. If `Data` is null despite `IsSuccess` being true (e.g., deserialization issue or API contract violation), this throws a NullReferenceException with no helpful context.

## What Changes

Add a null check for `Data` before accessing `TrustLevel`. Show an appropriate error message if `Data` is unexpectedly null.

## Capabilities

### Modified

- RegisterViewModel handles null Data defensively after successful registration

## Impact

- Prevents a potential NRE crash during user registration
- Improves error reporting for unexpected API responses
- Low risk — adds a guard without changing happy-path behavior
