## Context

The password strength indicator in `RegisterViewModel` sets color values directly via `Color.FromArgb()`. This couples the ViewModel to presentation concerns and bypasses the XAML resource system.

## Goals / Non-Goals

**Goals:**
- Move all password strength colors into `Colors.xaml`
- Use XAML-native mechanisms (converter or data triggers) to apply colors
- Keep the ViewModel responsible only for strength level (enum or string), not color

**Non-Goals:**
- Changing the password strength algorithm
- Adding new strength levels beyond Weak/Medium/Strong

## Decisions

- Define three named colors in `Colors.xaml`: `PasswordStrengthWeak`, `PasswordStrengthMedium`, `PasswordStrengthStrong`
- Expose password strength as an enum or string property from the ViewModel
- Use a value converter (`PasswordStrengthToColorConverter`) or `DataTrigger` in XAML to map strength level to color
- Prefer data triggers if the mapping is simple (3 discrete values)

## Risks / Trade-offs

- **Data triggers vs converter:** Triggers are simpler for 3 values but converters scale better if more levels are added later. Favor triggers for now.
- **Theme support:** Colors defined in `Colors.xaml` can have light/dark variants via `AppThemeBinding` if needed in the future.
