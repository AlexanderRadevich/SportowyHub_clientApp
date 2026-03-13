# Adopt Result Pattern -- Design

## Context

The codebase has two error-handling strategies: `AuthResult<T>` for auth operations and raw exception propagation for everything else. The architecture spec mandates a unified `Result<T>` pattern.

## Goals / Non-Goals

### Goals

- Single `Result<T>` type used by all services
- ViewModels never catch service exceptions directly
- Error messages flow through `Result.Failure` with contextual information

### Non-Goals

- Adding error codes or error categorization (future enhancement)
- Changing HTTP-level retry/resilience (handled by Polly)

## Decisions

- Place `Result<T>` in a shared `Models/` or root namespace so all layers can reference it
- Use static factory methods: `Result.Success(value)`, `Result.Failure<T>(message)`
- Provide `IsSuccess`, `Value`, and `ErrorMessage` properties
- Pilot with `ListingsService` before rolling out to remaining services
- Replace `AuthResult<T>` with `Result<T>` as final step to avoid two custom result types

## Risks / Trade-offs

- **Wide refactor surface:** Mitigated by piloting one service first
- **Merge conflicts:** Other in-flight changes touching services will need rebasing
- **Overuse:** Not every method needs `Result<T>` -- simple void operations can throw for truly exceptional cases
