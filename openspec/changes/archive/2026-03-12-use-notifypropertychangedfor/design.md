# Use NotifyPropertyChangedFor -- Design

## Context

CommunityToolkit.Mvvm provides `[NotifyPropertyChangedFor]` to automatically raise `PropertyChanged` for dependent properties when a source `[ObservableProperty]` changes. Two ViewModels use manual `OnPropertyChanged` calls instead.

## Goals / Non-Goals

### Goals

- Replace manual `OnPropertyChanged` calls with `[NotifyPropertyChangedFor]` attributes
- Leverage the source generator for consistent, error-free notification

### Non-Goals

- Refactoring ViewModel structure or property dependencies
- Adding new computed properties

## Decisions

- Add `[NotifyPropertyChangedFor(nameof(ComputedProp))]` to each `[ObservableProperty]` field that triggers dependent property updates
- Remove the manual `OnPropertyChanged` calls from partial method overrides or property setters
- If the manual calls are inside `partial void On<Property>Changed`, the entire partial method can be removed if it contains only `OnPropertyChanged` calls

## Risks / Trade-offs

- **Notification order:** The toolkit fires dependent notifications after the source property notification. If ordering matters, verify UI behavior.
- **Partial method removal:** If `On<Property>Changed` contains other logic beyond `OnPropertyChanged`, only remove the notification calls, not the entire method
