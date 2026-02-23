## 1. Update App Icon Assets

- [x] 1.1 Replace `Resources/AppIcon/appicon.png` with the updated version from `maui_assets/Resources/AppIcon/appicon.png` (solid white background)
- [x] 1.2 Copy `maui_assets/Resources/AppIcon/appiconfg.png` (foreground layer) to `Resources/AppIcon/appiconfg.png`

## 2. Update Project File

- [x] 2.1 Update `.csproj` `MauiIcon` entry: change from `<MauiIcon Include="Resources\AppIcon\appicon.png" Color="#FFFFFF" />` to `<MauiIcon Include="Resources\AppIcon\appicon.png" ForegroundFile="Resources\AppIcon\appiconfg.png" />`

## 3. Verify

- [x] 3.1 Build the project and verify it compiles without errors
