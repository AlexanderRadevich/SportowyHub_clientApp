# Design: Replace Static ViewModel State

## Context

`SearchViewModel` exposes `internal static Section? PendingSportSection` which `HomeViewModel` sets before navigating to the search page. This shared mutable static state is thread-unsafe, creates tight coupling, and violates MVVM.

## Goals / Non-Goals

### Goals
- Replace static field with WeakReferenceMessenger-based communication
- Maintain the same user-facing behavior (navigate to search with pre-selected sport)
- Ensure type-safe, testable cross-VM messaging

### Non-Goals
- Refactor the entire navigation pattern
- Change how sport sections are loaded or displayed

## Decisions

1. **WeakReferenceMessenger** — Use CommunityToolkit.Mvvm's `WeakReferenceMessenger.Default` for decoupled messaging. This is the recommended replacement for cross-VM communication.
2. **ValueChangedMessage pattern** — Create `NavigateToSearchMessage : ValueChangedMessage<Section>` to carry the sport section data.
3. **Register in constructor, unregister on deactivation** — SearchViewModel registers for the message in its constructor and unregisters when appropriate to prevent stale handlers.

## Risks / Trade-offs

- **Message ordering** — If the message is sent before SearchViewModel is constructed, it will be missed. Must ensure the VM is registered before navigation triggers the send. Alternative: send after navigation with a short delay, or use query parameters.
- **Query parameter alternative** — Shell query parameters (`?sectionId=5`) would be simpler and more idiomatic for navigation-based data passing. However, messaging is chosen here as it avoids serialization and supports complex objects.
