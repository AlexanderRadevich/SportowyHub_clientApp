## Context

The backend Symfony application stores listing prices as `DECIMAL(10,2)` in the database. Doctrine ORM maps this to PHP `?string` to avoid floating-point precision loss. The detail endpoint (`GET /api/v1/listings/{id}`) returns this directly as a JSON string `"150.00"`. However, the list endpoint (`GET /api/v1/listings`) fetches from Elasticsearch, where the indexer casts price to `float`, so it returns a JSON number `150.0`. The favorites endpoint follows the same Doctrine pattern (string). The app's `System.Text.Json` source-generated deserializer rejects type mismatches, causing runtime crashes.

## Goals / Non-Goals

**Goals:**
- Deserialize `price` correctly regardless of whether the backend sends a JSON number or JSON string
- Use `decimal?` as the C# type for price fields to preserve precision
- Fix all callers that assumed `Price` was `string?`

**Non-Goals:**
- Fixing the backend inconsistency (separate concern, different repo)
- Changing how prices are displayed in the UI (existing XAML bindings work with `decimal?`)
- Adding price formatting or currency conversion logic

## Decisions

**Decision 1: Use `decimal?` instead of `string?` for Price properties**
- Rationale: `decimal` is the correct C# type for monetary values — avoids floating-point precision issues that `float`/`double` introduce. Enables numeric formatting in XAML via `StringFormat`.
- Alternative considered: Keep `string?` and parse when needed — rejected because it pushes conversion logic into every consumer.

**Decision 2: Property-level `[JsonConverter]` attribute instead of global converter registration**
- Rationale: Source-generated `JsonSerializerContext` does not support runtime `JsonSerializerOptions.Converters`. The `[JsonConverter]` attribute on record constructor parameters (via `[property:]` target) works with source generation.
- Alternative considered: Switching to reflection-based serialization — rejected due to trimming/AOT compatibility and performance.

**Decision 3: Single `FlexibleDecimalConverter` class handling both number and string tokens**
- Rationale: Centralizes the dual-format handling in one place. The converter reads the `Utf8JsonReader.TokenType` and branches on `Number` vs `String` vs `Null`.
- Alternative considered: Two separate converters — rejected as unnecessary complexity for the same logical operation.

## Risks / Trade-offs

- [Risk] Backend may change price format again → Mitigation: The converter already handles number, string, and null — covers all reasonable JSON representations of a decimal value.
- [Risk] Other fields may have similar inconsistencies → Mitigation: The converter is reusable and can be applied to any `decimal?` property via `[JsonConverter]`.
- [Trade-off] Property-level attribute adds per-model boilerplate → Acceptable given it's only 3 properties and keeps source generation compatibility.
