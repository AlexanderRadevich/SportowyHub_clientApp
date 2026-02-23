# App Branding

### Requirement: Branded app icon
The app SHALL use MAUI's two-layer icon system for the app icon. The background layer (`appicon.png`, 1024x1024, solid white background) SHALL be the `MauiIcon` Include source. The foreground layer (`appiconfg.png`, S logo with safe-zone padding) SHALL be specified via the `ForegroundFile` attribute. No `Color` attribute SHALL be used since the background color is baked into `appicon.png`. Both files SHALL be located in `Resources/AppIcon/`.

#### Scenario: App icon displays SportowyHub branding on Android
- **WHEN** the app is installed on an Android device
- **THEN** the app icon SHALL display the SportowyHub S logo on a white background with no black circle artifact

#### Scenario: App icon displays SportowyHub branding on iOS
- **WHEN** the app is installed on an iOS device
- **THEN** the app icon SHALL display the SportowyHub branded icon with a white background (no transparency)

#### Scenario: Two-layer icon files are present
- **WHEN** the project `Resources/AppIcon/` directory is inspected
- **THEN** it SHALL contain `appicon.png` (white background layer) and `appiconfg.png` (foreground logo layer)

#### Scenario: csproj uses ForegroundFile attribute
- **WHEN** the `.csproj` file is inspected
- **THEN** the `MauiIcon` entry SHALL include `ForegroundFile="Resources\AppIcon\appiconfg.png"` and SHALL NOT include a `Color` attribute

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
