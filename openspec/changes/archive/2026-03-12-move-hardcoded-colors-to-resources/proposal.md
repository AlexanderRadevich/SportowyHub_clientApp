## Why

`RegisterViewModel.cs` contains hardcoded color values (`#16A34A`, `#F59E0B`, `#DC2626`) for the password strength indicator. Colors belong in XAML resources, not in ViewModel code. This violates separation of concerns and makes theming impossible.

## What Changes

Move password strength colors to `Colors.xaml` as named resources and use XAML data triggers or a converter to apply them based on strength level, removing color logic from the ViewModel.

## Capabilities

### New
- Named color resources for password strength levels (Weak, Medium, Strong)
- XAML-based color selection via converter or data triggers

### Modified
- `RegisterViewModel.cs` — remove `Color` properties for password strength
- `RegisterPage.xaml` — bind strength indicator color via XAML mechanisms
- `Colors.xaml` — add `PasswordStrengthWeak`, `PasswordStrengthMedium`, `PasswordStrengthStrong` colors

## Impact

- **Theming:** Password strength colors become theme-aware
- **Testability:** ViewModel no longer depends on `Microsoft.Maui.Graphics.Color`
- **Risk:** Low — visual-only change with clear before/after verification
