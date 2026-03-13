## Context

CommunityToolkit.Maui is already a project dependency and provides a set of standard converters. The project has custom converters that duplicate this functionality.

## Goals / Non-Goals

**Goals:**
- Replace `InvertBoolConverter` with `InvertedBoolConverter` from toolkit
- Replace `IsNotNullConverter` with `IsNotNullConverter` from toolkit
- Replace `IntToBoolConverter` with toolkit equivalent (e.g., `IsEqualConverter` or `IsNotEqualConverter`)
- Remove the custom converter files

**Non-Goals:**
- Replacing converters that have no toolkit equivalent
- Adding new converters

## Decisions

- Audit all XAML files for references to each custom converter before replacing
- Update `App.xaml` resource dictionary to register toolkit converters with the same keys (to minimize XAML changes)
- If toolkit converters use different keys, update all XAML references
- Delete the custom converter `.cs` files after successful replacement

## Risks / Trade-offs

- **Behavioral differences:** Toolkit converters may handle edge cases (null, unexpected types) differently. Verify each replacement behaves identically.
- **Key naming:** Using the same resource keys as before minimizes XAML churn but may confuse developers expecting the custom implementation.
