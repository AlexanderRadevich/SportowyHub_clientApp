# Tasks: Fix Chip Theme Reactivity

## Tasks

- [x] Replace hardcoded color values with `AppThemeBinding` in `CreateSectionChip`
- [x] Replace hardcoded color values with `AppThemeBinding` in `StyleChip`/`UpdateChipStyles`
- [x] Define chip styles (selected/unselected) in `Styles.xaml` as named styles
- [x] Test theme toggle updates chips in real-time without navigating away
- [x] Consider moving chip creation from code-behind to XAML `DataTemplate` with `BindableLayout`
- [x] Test on both Android and iOS for theme change reactivity
