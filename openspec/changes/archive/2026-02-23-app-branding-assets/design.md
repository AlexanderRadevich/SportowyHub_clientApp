## Context

The app currently uses the default .NET MAUI template assets: SVG-based app icon (`appicon.svg` + `appiconfg.svg`) with purple `#512BD4`, an SVG splash screen with the same purple background, and `dotnet_bot.png` as a placeholder logo. The `maui_assets/` directory contains ready-to-use SportowyHub branded PNGs at all required sizes and platform variants.

MAUI's asset pipeline supports both SVG and PNG for `MauiIcon` and `MauiSplashScreen`. The current SVGs are template defaults that need to be swapped with the branded PNGs.

## Goals / Non-Goals

**Goals:**
- Replace all template branding with SportowyHub assets across all platforms
- Use MAUI's built-in asset pipeline (`MauiIcon`, `MauiSplashScreen`, `MauiImage`) for cross-platform icon/splash generation
- Add in-app logo images for use in UI (profile screen, future headers)

**Non-Goals:**
- No adaptive icon configuration (Android foreground/background layers) beyond what MAUI generates from the single PNG
- No platform-specific code changes — asset pipeline handles platform adaptation
- No manual placement of platform-specific pre-sized icons in this phase (MAUI generates these from the source PNG; the `Platform_Specific/` folder in `maui_assets/` is kept as reference but not manually copied)

## Decisions

### 1. Use MAUI Asset Pipeline, Not Manual Platform Icons

**Decision:** Use `MauiIcon` and `MauiSplashScreen` with the source PNGs. Do NOT manually copy platform-specific icons from `maui_assets/Platform_Specific/` into `Platforms/Android/Resources/` or iOS bundles.

**Rationale:** MAUI's build pipeline automatically generates all required platform-specific icon sizes from the single source PNG. Manual placement would conflict with the auto-generated outputs and create maintenance burden. The `Platform_Specific/` assets in `maui_assets/` serve as reference for what the pipeline should produce.

**Alternative considered:** Manually place pre-sized PNGs in each platform's resource folder — gives pixel-perfect control but bypasses MAUI's pipeline, creates duplication, and requires manual updates on every icon change.

### 2. PNG Over SVG for App Icon

**Decision:** Replace the SVG `MauiIcon` with PNG source.

```xml
<!-- Before -->
<MauiIcon Include="Resources\AppIcon\appicon.svg" ForegroundFile="Resources\AppIcon\appiconfg.svg" Color="#512BD4" />

<!-- After -->
<MauiIcon Include="Resources\AppIcon\appicon.png" />
```

**Rationale:** The provided `appicon.png` is a pre-designed 1024x1024 composite icon (not a foreground+background pair). Using a single PNG is simpler and matches the provided assets. No `ForegroundFile` or `Color` needed since the icon is already composited.

### 3. iOS Icon Variant

**Decision:** Use `appicon_ios.png` (white background, no transparency) for iOS by adding a conditional `MauiIcon` or by relying on the standard `appicon.png` (MAUI composites transparency onto white for iOS automatically).

**Rationale:** iOS App Store rejects icons with transparency. MAUI's pipeline fills transparent areas with the specified `Color` attribute. Since our `appicon.png` has a transparent background, we have two options:
- Set `Color="#FFFFFF"` on `MauiIcon` to fill transparency with white on all platforms
- The provided `appicon_ios.png` with baked-in white background is a reference; MAUI handles this automatically with the `Color` attribute

**Decision:** Use `appicon.png` with `Color="#FFFFFF"` — simplest approach, one icon source, MAUI handles platform differences.

### 4. Splash Screen Configuration

**Decision:** Replace SVG splash with PNG.

```xml
<MauiSplashScreen Include="Resources\Splash\splash.png" Color="#FFFFFF" BaseSize="620,157" />
```

**Rationale:** The `splash.png` is the full "SportowyHub" text logo. White background matches the brand's light theme. `BaseSize="620,157"` matches the logo spec dimensions so MAUI scales it correctly on all screen sizes.

### 5. In-App Logo Usage

**Decision:** Add `logo_full.png` and height variants to `Resources/Images/`. Use `logo_full.png` as the primary in-app logo (ProfilePage). Height variants (`logo_full_40h.png`, etc.) are available for different contexts but the main `logo_full.png` with MAUI's `HeightRequest` handles most cases.

**Rationale:** MAUI's image system handles scaling from a single source. The height variants provide pre-optimized options if pixel-perfect rendering at specific sizes is needed, but `HeightRequest` on the `Image` control is sufficient for MVP.

### 6. Cleanup of Template Assets

**Decision:** Remove `appicon.svg`, `appiconfg.svg`, `splash.svg`, and `dotnet_bot.png`. Remove the `dotnet_bot.png` resize entry from `.csproj`.

**Rationale:** These are template leftovers. Keeping them would cause confusion and the SVG MauiIcon entry would conflict with the new PNG entry.

## Risks / Trade-offs

- **MAUI auto-generated icon quality** → The pipeline may produce slightly different results than the hand-crafted platform-specific PNGs in `maui_assets/Platform_Specific/`. Mitigation: Acceptable for MVP; if quality issues arise on specific platforms, platform-specific overrides can be added later.

- **Splash screen scaling** → The `BaseSize` must match the actual image dimensions for correct scaling. If `splash.png` dimensions differ from `620,157`, the splash may appear stretched or cropped. Mitigation: Verify actual dimensions of `splash.png` before setting `BaseSize`.

- **Transparent icon on Android** → Android adaptive icons may render the transparent background oddly depending on launcher. Mitigation: `Color="#FFFFFF"` fills the background, giving a clean white icon on all platforms.
