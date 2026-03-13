## Context

`InvertIntToBoolConverter` is registered in `App.xaml` but has no consumers. It adds dead code to the project.

## Goals / Non-Goals

**Goals:**
- Confirm zero references to `InvertIntToBoolConverter` in XAML and code
- Remove the converter class and its `App.xaml` registration

**Non-Goals:**
- Auditing other converters for usage (covered by S10)

## Decisions

- Use find-references tooling to confirm no usages exist
- Remove the `ResourceDictionary` entry from `App.xaml`
- Delete the converter `.cs` file

## Risks / Trade-offs

- **False negative:** If the converter is referenced via string-based lookup (unlikely in MAUI), removal would cause a runtime error. Mitigated by searching all files for the converter name.
