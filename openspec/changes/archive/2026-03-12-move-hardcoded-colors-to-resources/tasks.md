## Tasks

- [x] Add `PasswordStrengthWeak` (#DC2626), `PasswordStrengthMedium` (#F59E0B), `PasswordStrengthStrong` (#16A34A) colors to `Colors.xaml`
- [x] Add a password strength enum or string property to `RegisterViewModel` (replace Color properties)
- [x] Create data triggers or converter in `RegisterPage.xaml` to map strength level to color
- [x] Remove `Color.FromArgb()` calls and Color properties from `RegisterViewModel.cs`
- [x] Verify password strength indicator displays correct colors for each level
- [x] Test in both light and dark themes
