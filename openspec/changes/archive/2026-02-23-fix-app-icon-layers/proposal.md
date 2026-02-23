## Why

The app icon renders with a black circle on Android devices. The current `appicon.png` has a transparent background, and Android's adaptive icon system fills transparent areas with black. This needs to be fixed by switching to MAUI's two-layer icon system (background + foreground) so Android draws a white background behind the logo.

## What Changes

- Replace the single-layer `MauiIcon` (appicon.png with `Color="#FFFFFF"`) with a two-layer setup: `appicon.png` (solid white background) + `appiconfg.png` (foreground S logo with safe-zone padding)
- Copy the updated `appicon.png` (now white background, not transparent) from `maui_assets/`
- Copy the new `appiconfg.png` (foreground layer) from `maui_assets/`
- Update `.csproj` to use `ForegroundFile="Resources\AppIcon\appiconfg.png"` and remove the `Color` attribute (background is baked into `appicon.png`)

## Capabilities

### New Capabilities
<!-- None — this is a fix to an existing capability -->

### Modified Capabilities
- `app-branding`: The "Branded app icon" requirement changes from a single-PNG approach with `Color="#FFFFFF"` to a two-layer foreground/background approach using `ForegroundFile`

## Impact

- **Resources/AppIcon/appicon.png**: Replaced with new version (solid white background instead of transparent)
- **Resources/AppIcon/appiconfg.png**: New file — foreground layer with S logo and safe-zone padding
- **SportowyHub.csproj**: `MauiIcon` entry updated from `<MauiIcon Include="..." Color="#FFFFFF" />` to `<MauiIcon Include="..." ForegroundFile="..." />`
- No code changes, no page changes, no other files affected
