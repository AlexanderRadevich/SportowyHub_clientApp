# Use NotifyPropertyChangedFor Attribute

## Why

`AccountProfileViewModel.cs` has 6 manual `OnPropertyChanged` calls and `ListingDetailViewModel.cs` has 3. These computed properties depend on another `[ObservableProperty]` and should use the `[NotifyPropertyChangedFor]` source generator attribute instead of manual invocation. Manual calls are error-prone and bypass the toolkit's code generation.

## What Changes

Add `[NotifyPropertyChangedFor(nameof(...))]` attributes to the source properties and remove the manual `OnPropertyChanged` calls.

## Capabilities

### New

- None

### Modified

- `AccountProfileViewModel.cs` -- add `[NotifyPropertyChangedFor]` to source property, remove 6 manual calls
- `ListingDetailViewModel.cs` -- add `[NotifyPropertyChangedFor]` to source property, remove 3 manual calls

## Impact

- **Scope:** Two ViewModel files
- **Risk:** Very low -- replacing manual calls with the toolkit's built-in mechanism
- **Testing:** Verify dependent properties still raise change notifications
- **UX:** No user-visible changes; UI bindings continue to update correctly
