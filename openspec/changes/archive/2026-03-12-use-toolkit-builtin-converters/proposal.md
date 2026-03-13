## Why

The project has custom `InvertBoolConverter`, `IsNotNullConverter`, and `IntToBoolConverter` that duplicate built-in converters already available in CommunityToolkit.Maui (which is referenced in the project).

## What Changes

Replace custom converters with their CommunityToolkit.Maui equivalents: `InvertedBoolConverter`, `IsNotNullConverter`, and `IsEqualConverter`. Remove the custom converter files.

## Capabilities

### New
- None (using existing toolkit converters)

### Modified
- `App.xaml` — replace custom converter registrations with toolkit built-ins
- All XAML files referencing the custom converters — update resource keys if needed
- Delete `InvertBoolConverter.cs`, `IsNotNullConverter.cs`, `IntToBoolConverter.cs`

## Impact

- **Maintenance:** Less custom code to maintain; leverages well-tested toolkit implementations
- **Consistency:** Aligns with toolkit conventions used elsewhere in the project
- **Risk:** Low — requires verifying all XAML references are updated and behavior matches
