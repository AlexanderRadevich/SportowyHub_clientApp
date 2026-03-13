## Why

`InvertIntToBoolConverter` is registered in `App.xaml` (line 15) but is not referenced in any XAML file or code-behind. Dead code adds confusion and maintenance burden.

## What Changes

Remove the `InvertIntToBoolConverter` class file and its registration from `App.xaml`.

## Capabilities

### New
- None

### Modified
- `App.xaml` — remove converter registration
- Delete `InvertIntToBoolConverter.cs`

## Impact

- **Cleanliness:** Removes dead code from the project
- **Risk:** Minimal — verify no references exist before deletion
