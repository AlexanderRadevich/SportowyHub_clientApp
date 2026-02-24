## 1. Resource File Infrastructure

- [x] 1.1 Create `Resources/Strings/` directory and add base `AppResources.resx` with Polish translations for all ~28 resource keys
- [x] 1.2 Create `AppResources.en.resx` with English translations for all keys
- [x] 1.3 Create `AppResources.uk.resx` with Ukrainian translations for all keys
- [x] 1.4 Create `AppResources.ru.resx` with Russian translations for all keys

## 2. Language Detection and Fallback

- [x] 2.1 Add culture detection logic in `App.xaml.cs` constructor — check `CultureInfo.CurrentUICulture` against supported languages (pl, en, uk, ru) and override to Polish if unsupported

## 3. Replace Hardcoded Strings in XAML

- [x] 3.1 Update `AppShell.xaml` — replace tab titles (Home, Search, Favorites, Profile) with `{x:Static}` references to `AppResources`
- [x] 3.2 Update `LoginPage.xaml` — replace page title, field labels, placeholders, button text, and link text with `{x:Static}` references
- [x] 3.3 Update `RegisterPage.xaml` — replace page title, field labels, placeholders, and button text with `{x:Static}` references
- [x] 3.4 Update `HomePage.xaml` — replace search placeholder and content text with `{x:Static}` references
- [x] 3.5 Update `SearchPage.xaml` — replace search placeholder and section headers with `{x:Static}` references
- [x] 3.6 Update `FavoritesPage.xaml` — replace page title with `{x:Static}` reference
- [x] 3.7 Update `ProfilePage.xaml` — replace welcome text, button labels with `{x:Static}` references

## 4. Replace Hardcoded Strings in C#

- [x] 4.1 Update `RegisterViewModel.cs` — replace validation messages ("Please enter a valid email address", "Passwords do not match") and password strength labels ("Strong", "Medium", "Weak") with `AppResources` references
- [x] 4.2 Update `SearchViewModel.cs` — replace section headers ("Recent Searches", "Popular Searches") with `AppResources` references if exposed as ViewModel properties

## 5. Build Verification

- [x] 5.1 Verify the project builds successfully on all target frameworks with no hardcoded user-facing strings remaining in XAML or C# files
