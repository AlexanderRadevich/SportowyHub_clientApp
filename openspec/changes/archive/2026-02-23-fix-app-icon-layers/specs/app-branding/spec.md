## MODIFIED Requirements

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
