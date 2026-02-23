## ADDED Requirements

### Requirement: Branded app icon
The app SHALL use the SportowyHub branded PNG icon (`appicon.png`, 1024x1024) as the source for `MauiIcon` in the `.csproj`. The `MauiIcon` entry SHALL specify `Color="#FFFFFF"` to fill transparent areas with white for iOS compatibility. The default template SVG icons (`appicon.svg`, `appiconfg.svg`) SHALL be removed.

#### Scenario: App icon displays SportowyHub branding on Android
- **WHEN** the app is installed on an Android device
- **THEN** the app icon SHALL display the SportowyHub branded icon (not the default .NET purple icon)

#### Scenario: App icon displays SportowyHub branding on iOS
- **WHEN** the app is installed on an iOS device
- **THEN** the app icon SHALL display the SportowyHub branded icon with a white background (no transparency)

#### Scenario: Template SVG icons are removed
- **WHEN** the project `Resources/AppIcon/` directory is inspected
- **THEN** it SHALL contain only `appicon.png` (no `appicon.svg` or `appiconfg.svg`)

### Requirement: Branded splash screen
The app SHALL use the SportowyHub splash screen PNG (`splash.png`) as the source for `MauiSplashScreen` in the `.csproj`. The splash screen SHALL have a white background (`Color="#FFFFFF"`) and `BaseSize` matching the logo dimensions. The default template SVG splash (`splash.svg`) SHALL be removed.

#### Scenario: Splash screen displays on app launch
- **WHEN** the app is launched
- **THEN** the splash screen SHALL display the SportowyHub logo on a white background

#### Scenario: Template SVG splash is removed
- **WHEN** the project `Resources/Splash/` directory is inspected
- **THEN** it SHALL contain only `splash.png` (no `splash.svg`)

### Requirement: In-app logo images
The project SHALL include the SportowyHub logo images in `Resources/Images/` for use in UI screens: `logo_full.png` (primary full logo) and height variants (`logo_full_40h.png`, `logo_full_60h.png`, `logo_full_80h.png`, `logo_full_120h.png`). Icon-only variants (`icon_32.png` through `icon_512.png`) SHALL also be included for future use in tabs, buttons, and other UI elements.

#### Scenario: Logo images are available as MAUI image resources
- **WHEN** a XAML page references `Source="logo_full.png"`
- **THEN** the image SHALL resolve and render the SportowyHub logo

#### Scenario: Icon images are available at multiple sizes
- **WHEN** the `Resources/Images/` directory is inspected
- **THEN** it SHALL contain `icon_32.png`, `icon_48.png`, `icon_64.png`, `icon_128.png`, `icon_256.png`, and `icon_512.png`

### Requirement: Template placeholder assets removed
The default .NET MAUI template asset `dotnet_bot.png` SHALL be removed from `Resources/Images/`. Any `.csproj` entries referencing `dotnet_bot.png` (including resize metadata) SHALL be removed.

#### Scenario: dotnet_bot.png is not present
- **WHEN** the `Resources/Images/` directory is inspected
- **THEN** `dotnet_bot.png` SHALL NOT be present

#### Scenario: csproj has no dotnet_bot reference
- **WHEN** the `.csproj` file is inspected
- **THEN** there SHALL be no `MauiImage` entry referencing `dotnet_bot.png`
