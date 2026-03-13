# Design: Propagate Cancellation Tokens

## Context

CommunityToolkit.Mvvm's `[RelayCommand]` source generator automatically provides a `CancellationToken` to async command methods that accept one. The generated command cancels the token when `Cancel()` is called or when the command is re-invoked (for non-concurrent commands). Several ViewModels do not take advantage of this.

## Goals / Non-Goals

### Goals
- All async `[RelayCommand]` methods accept `CancellationToken` parameter
- Tokens are propagated to every service method and HTTP call in the chain
- Replace all `CancellationToken.None` usages with the propagated token

### Non-Goals
- Adding page-level cancellation on navigation (separate concern — requires IDisposable)
- Adding cancellation UI (cancel buttons)

## Decisions

1. **Use toolkit-generated token** — add `CancellationToken ct` as the last parameter of async relay command methods; the source generator handles the rest
2. **Service interfaces accept optional token** — `CancellationToken ct = default` on service methods that lack it
3. **No try/catch for OperationCanceledException** — the toolkit command infrastructure handles this automatically

## Risks / Trade-offs

- **Service interface changes:** Adding `CancellationToken` parameter to service interfaces is a breaking change for implementations and tests. Mitigated by using `= default` so existing callers compile.
- **Partial cancellation:** Some operations (e.g., database writes) should not be cancelled mid-way. These should explicitly ignore the token for the write portion.
