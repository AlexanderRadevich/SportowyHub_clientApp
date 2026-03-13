# Fix RequestProvider Null Safety

## Why

Every deserialization call in `RequestProvider` uses the `!` null-forgiving operator. If the API returns an empty body or `null` JSON, this causes a `NullReferenceException` with no diagnostic context — the stack trace points to the caller, not the deserialization site.

## What Changes

Add explicit null checks after each `JsonSerializer.Deserialize` call. Throw `InvalidOperationException` with a descriptive message including the expected type name when deserialization returns null.

## Capabilities

### Modified

- `RequestProvider` — all 6 deserialization sites (lines 21, 45, 61, 77, 93, 107) get null checks

## Impact

- **Debugging:** Null responses produce clear, actionable exceptions instead of cryptic NREs
- **Risk:** Low — changes failure mode from NRE to InvalidOperationException (both are exceptions, but the new one is informative)
- **Breaking changes:** None — callers already need to handle exceptions
