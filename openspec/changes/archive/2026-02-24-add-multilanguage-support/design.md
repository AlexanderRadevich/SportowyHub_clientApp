## Context

SportowyHub is a .NET MAUI app targeting Android, iOS, macCatalyst, and Windows. It uses MVVM with CommunityToolkit.Mvvm, XAML views with compiled bindings (`x:DataType`), and XAML source generation (`MauiXamlInflator=SourceGen`).

Currently, all ~34 user-facing strings are hardcoded in English across 7 XAML files and 2 C# ViewModels. There is no localization infrastructure. The app needs to support 4 languages: Polish (pl), English (en), Ukrainian (uk), and Russian (ru), with automatic detection from system settings and Polish as the fallback.

**Constraints:**
- Must work across all four target platforms
- Must work with XAML source generation (SourceGen inflator)
- No additional NuGet packages desired — use built-in .NET mechanisms
- The app uses `CommunityToolkit.Mvvm` with `[ObservableProperty]` source generators

## Goals / Non-Goals

**Goals:**
- Provide a localization infrastructure that automatically detects the device language
- Fall back to Polish when the system language is not among supported languages
- Translate all existing user-facing strings into pl, en, uk, ru
- Make it easy to add new localizable strings going forward
- Support localization in both XAML and C# code

**Non-Goals:**
- Runtime language switching (user picks a language in-app settings) — out of scope for this change
- Localizing app store metadata, app name, or platform-specific resources (e.g., Android `strings.xml`)
- Right-to-left (RTL) layout support
- Pluralization or parameterized string formatting beyond simple string resources
- Translating search suggestion placeholder data (these are mock data, not real UI labels)

## Decisions

### 1. Use `.resx` resource files with `ResourceManager` (standard .NET approach)

**Choice:** Standard `.resx` resource files with the built-in `ResourceManager` pattern.

**Rationale:** .NET MAUI inherits full .NET resource support. `.resx` files are the established pattern, require no extra packages, work with all target platforms, and integrate with `CultureInfo.CurrentUICulture` automatically. The generated strongly-typed `AppResources` class provides compile-time safety.

**Alternatives considered:**
- `Microsoft.Extensions.Localization` (`IStringLocalizer`) — adds complexity with DI registration and requires injecting localizers into every ViewModel. Overkill for a straightforward case with ~34 strings and no runtime language switching.
- JSON-based localization — no built-in MAUI support, would require a custom solution.

### 2. XAML consumption via `x:Static` markup extension

**Choice:** Reference strings in XAML using `x:Static` pointing to the generated `AppResources` class properties.

```xml
<Label Text="{x:Static res:AppResources.SignIn}" />
```

**Rationale:** This is the simplest approach that works with XAML source generation. It requires only adding an `xmlns:res` namespace declaration to each XAML file. No custom markup extensions needed.

**Alternatives considered:**
- Custom `TranslateExtension` markup extension — adds unnecessary abstraction for our case. Useful when runtime language switching is needed (not a goal).
- Binding to a localization ViewModel — adds coupling and complexity without benefit.

### 3. C# consumption via direct static access

**Choice:** In ViewModels and code-behind, access strings via `AppResources.PropertyName`.

```csharp
EmailError = AppResources.InvalidEmail;
```

**Rationale:** The `.resx` designer generates a static class with static properties. Direct access is the simplest and most readable approach. No DI or service injection needed.

### 4. Language detection and fallback strategy

**Choice:** Set `CultureInfo.CurrentUICulture` at app startup based on system culture, with fallback to Polish.

**Implementation:** In `App.xaml.cs` constructor (before any UI loads):
1. Read `CultureInfo.CurrentUICulture` (populated by each platform from system settings)
2. Check if the language code (`pl`, `en`, `uk`, `ru`) matches a supported language
3. If not, override `CurrentUICulture` to `pl` (Polish)
4. The `ResourceManager` automatically resolves the correct `.resx` file based on `CurrentUICulture`

**Rationale:** Setting the culture once at startup is sufficient since runtime switching is a non-goal. `App.xaml.cs` runs before any page is constructed, ensuring all UI sees the correct culture from the start.

**Alternatives considered:**
- Platform-specific language detection (Android `Locale`, iOS `NSLocale`) — unnecessary; `CultureInfo.CurrentUICulture` already abstracts this cross-platform.
- A `LocalizationService` singleton — overkill when we only need startup-time detection with no runtime switching.

### 5. Resource file structure

**Choice:** Place resource files in a `Resources/Strings/` directory:

```
Resources/
  Strings/
    AppResources.resx          (Polish — default/fallback)
    AppResources.en.resx       (English)
    AppResources.uk.resx       (Ukrainian)
    AppResources.ru.resx       (Russian)
```

**Rationale:** Polish is the default fallback, so the base (neutral) `.resx` contains Polish translations. This means any unsupported culture automatically gets Polish — matching the requirement. English, Ukrainian, and Russian are satellite resource files.

### 6. Resource key naming convention

**Choice:** PascalCase keys grouped by feature prefix:

- `TabHome`, `TabSearch`, `TabFavorites`, `TabProfile`
- `AuthSignIn`, `AuthEmail`, `AuthPassword`, `AuthEnterEmail`, `AuthEnterPassword`, `AuthForgotPassword`, `AuthLogin`, `AuthNoAccount`, `AuthCreateAccount`
- `AuthConfirmPassword`, `AuthConfirmYourPassword`, `AuthInvalidEmail`, `AuthPasswordsDoNotMatch`
- `PasswordStrengthStrong`, `PasswordStrengthMedium`, `PasswordStrengthWeak`
- `HomeTitle`, `HomeContentComingSoon`
- `SearchPlaceholder`, `SearchRecentSearches`, `SearchPopularSearches`
- `FavoritesTitle`
- `ProfileWelcome`, `ProfileCreateAccount`, `ProfileSignIn`

**Rationale:** Prefix grouping keeps keys organized and avoids collisions. PascalCase matches C# property naming conventions since the designer generates properties from these keys.

## Risks / Trade-offs

**[Risk] XAML source generation compatibility** → The `x:Static` markup extension is well-supported by MAUI's XAML source generator. This is a standard pattern documented by Microsoft. Low risk.

**[Risk] Missing translations** → If a key is missing from a satellite `.resx`, `ResourceManager` falls back to the neutral (Polish) resource automatically. This is safe — worst case, a user sees Polish instead of a broken string.

**[Trade-off] No runtime language switching** → Users must change their system language setting and restart the app. This keeps the implementation simple. Runtime switching can be added later as a separate change if needed.

**[Trade-off] Search suggestions not translated** → Search suggestions ("Running shoes", "Basketball", etc.) are placeholder/mock data in `SearchViewModel`. Translating mock data has low value and will be replaced when real search is implemented. Section headers ("Recent Searches", "Popular Searches") will be translated.
