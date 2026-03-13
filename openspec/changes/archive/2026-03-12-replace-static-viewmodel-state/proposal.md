# Replace Static ViewModel State

## Why

`SearchViewModel.cs` has `internal static Section? PendingSportSection` set by `HomeViewModel`. This is thread-unsafe, creates tight coupling between view models, and breaks MVVM by using shared mutable static state for cross-VM communication.

## What Changes

Replace the static field with `WeakReferenceMessenger` from CommunityToolkit.Mvvm. Create a `NavigateToSearchMessage` record and use send/receive via the messenger.

## Capabilities

### New
- `NavigateToSearchMessage` record for type-safe cross-VM messaging

### Modified
- `HomeViewModel.cs` — send message instead of setting static field
- `SearchViewModel.cs` — receive message instead of reading static field; remove static field

## Impact

- **Architecture** — proper MVVM decoupling between view models
- **Reliability** — eliminates thread-safety issues with shared mutable state
- **Testability** — messaging can be verified in unit tests
