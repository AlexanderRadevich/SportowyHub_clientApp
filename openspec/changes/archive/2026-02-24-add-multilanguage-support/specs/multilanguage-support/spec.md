## ADDED Requirements

### Requirement: Resource-based localization infrastructure
The app SHALL use `.resx` resource files for all user-facing strings. A base resource file (`AppResources.resx`) SHALL contain Polish translations as the neutral/fallback language. Satellite resource files SHALL exist for English (`AppResources.en.resx`), Ukrainian (`AppResources.uk.resx`), and Russian (`AppResources.ru.resx`). All resource files SHALL be located in `Resources/Strings/`.

#### Scenario: Resource files exist for all supported languages
- **WHEN** the project is built
- **THEN** resource files SHALL exist for Polish (base), English (.en), Ukrainian (.uk), and Russian (.ru) in `Resources/Strings/`

#### Scenario: Polish is the neutral resource
- **WHEN** the `ResourceManager` resolves a string for an unsupported culture
- **THEN** it SHALL return the Polish translation from the base `AppResources.resx`

### Requirement: Automatic language detection from system settings
The app SHALL detect the device's current UI culture at startup via `CultureInfo.CurrentUICulture`. If the detected language matches one of the supported languages (pl, en, uk, ru), the app SHALL use that language. If the detected language is not supported, the app SHALL override `CultureInfo.CurrentUICulture` to Polish (`pl`).

#### Scenario: System language is a supported language
- **WHEN** the app starts and the device language is set to English (en)
- **THEN** the app SHALL display all UI strings in English

#### Scenario: System language is Polish
- **WHEN** the app starts and the device language is set to Polish (pl)
- **THEN** the app SHALL display all UI strings in Polish

#### Scenario: System language is Ukrainian
- **WHEN** the app starts and the device language is set to Ukrainian (uk)
- **THEN** the app SHALL display all UI strings in Ukrainian

#### Scenario: System language is Russian
- **WHEN** the app starts and the device language is set to Russian (ru)
- **THEN** the app SHALL display all UI strings in Russian

#### Scenario: System language is unsupported
- **WHEN** the app starts and the device language is not pl, en, uk, or ru (e.g., German, French)
- **THEN** the app SHALL fall back to Polish and display all UI strings in Polish

### Requirement: Language detection runs before UI loads
The culture detection and fallback logic SHALL execute in the `App` constructor, before any page or shell content is constructed, ensuring all UI elements see the correct culture from the start.

#### Scenario: Culture is set before first page renders
- **WHEN** the app starts
- **THEN** `CultureInfo.CurrentUICulture` SHALL be set to a supported culture before `MainPage` is assigned

### Requirement: XAML strings use x:Static resource references
All user-facing strings in XAML files SHALL be referenced via `{x:Static}` markup extension pointing to the generated `AppResources` class. No user-facing strings SHALL be hardcoded in XAML.

#### Scenario: XAML label uses localized string
- **WHEN** a XAML page is rendered
- **THEN** all `Text`, `Title`, and `Placeholder` attributes containing user-facing text SHALL use `{x:Static res:AppResources.KeyName}` syntax

### Requirement: C# strings use AppResources static properties
All user-facing strings in C# code (ViewModels, code-behind) SHALL be accessed via `AppResources.PropertyName` static properties. No user-facing strings SHALL be hardcoded in C# code.

#### Scenario: ViewModel validation message uses localized string
- **WHEN** a ViewModel produces a validation error message
- **THEN** the message text SHALL come from `AppResources` static properties

### Requirement: Resource key naming convention
All resource keys SHALL use PascalCase with a feature prefix. Tab-related keys SHALL use the `Tab` prefix. Auth-related keys SHALL use the `Auth` prefix. Password strength keys SHALL use the `PasswordStrength` prefix. Search-related keys SHALL use the `Search` prefix. Home-related keys SHALL use the `Home` prefix. Favorites-related keys SHALL use the `Favorites` prefix. Profile-related keys SHALL use the `Profile` prefix.

#### Scenario: Resource keys follow naming convention
- **WHEN** a new localizable string is added
- **THEN** its resource key SHALL follow the pattern `{FeaturePrefix}{DescriptiveName}` in PascalCase (e.g., `AuthSignIn`, `TabHome`, `PasswordStrengthWeak`)
