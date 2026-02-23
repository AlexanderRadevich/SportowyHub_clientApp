## Why

The app still ships with the default .NET MAUI template branding — purple `#512BD4` SVG app icon, purple splash screen, and `dotnet_bot.png` as the placeholder logo on the Profile screen. Pre-made SportowyHub brand assets (app icons, splash, logo variants) are ready in `maui_assets/` and need to be integrated so the app reflects the actual brand identity on all platforms.

## What Changes

- Replace the default SVG app icon (`appicon.svg` + `appiconfg.svg`) with the SportowyHub PNG app icon (`appicon.png`) and iOS-specific variant (`appicon_ios.png`)
- Replace the default SVG splash screen (`splash.svg` with purple background) with the SportowyHub branded splash (`splash.png` with white background)
- Add in-app logo images (`logo_full.png` + height variants) and icon images (`icon_32` through `icon_512`) to `Resources/Images/`
- Copy platform-specific pre-sized icons for Android (`mipmap-mdpi` through `mipmap-xxxhdpi`) and iOS (all required sizes)
- Update `.csproj` to reference PNG-based `MauiIcon` and `MauiSplashScreen` instead of SVG
- Replace `dotnet_bot.png` usage in ProfilePage with `logo_full.png`
- Remove obsolete template assets (`appicon.svg`, `appiconfg.svg`, `splash.svg`, `dotnet_bot.png`)

## Capabilities

### New Capabilities
- `app-branding`: App icon, splash screen, and in-app logo/icon image assets for all platforms

### Modified Capabilities
- `auth-screens`: Profile logged-out state changes the app logo image source from `dotnet_bot.png` to `logo_full.png`

## Impact

- **SportowyHub.csproj**: `MauiIcon` and `MauiSplashScreen` entries updated from SVG to PNG; `dotnet_bot.png` resize entry removed
- **Resources/AppIcon/**: SVG files replaced with PNG files
- **Resources/Splash/**: SVG replaced with PNG
- **Resources/Images/**: New logo and icon PNGs added; `dotnet_bot.png` removed
- **Platforms/Android/Resources/**: Pre-sized mipmap icons added
- **Platforms/iOS/**: Pre-sized iOS icons added (if manual placement needed)
- **Views/Profile/ProfilePage.xaml**: Image source changed from `dotnet_bot.png` to `logo_full.png`
