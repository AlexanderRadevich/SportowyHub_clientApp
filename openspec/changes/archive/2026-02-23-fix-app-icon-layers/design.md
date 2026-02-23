## Context

The current `.csproj` uses a single-layer icon approach:
```xml
<MauiIcon Include="Resources\AppIcon\appicon.png" Color="#FFFFFF" />
```

The `Color` attribute is meant to fill transparent areas, but Android's adaptive icon system processes the icon before MAUI's color fill takes effect on some launchers, resulting in a black circle behind the logo. The `appicon.png` in the project currently has a transparent background.

New assets have been prepared in `maui_assets/Resources/AppIcon/`:
- `appicon.png` — solid white background layer (1024x1024)
- `appiconfg.png` — foreground layer with S logo and extra safe-zone padding for adaptive icon cropping

## Goals / Non-Goals

**Goals:**
- Fix the black circle on Android by using MAUI's native two-layer icon system
- Ensure the icon renders correctly on both Android (circular crop) and iOS (rounded square)

**Non-Goals:**
- No changes to splash screen, in-app logos, or any other assets
- No changes to iOS-specific icon handling beyond what MAUI generates automatically

## Decisions

### 1. Two-Layer ForegroundFile Approach

**Decision:** Use MAUI's `ForegroundFile` attribute to split the icon into background + foreground layers.

```xml
<!-- Before -->
<MauiIcon Include="Resources\AppIcon\appicon.png" Color="#FFFFFF" />

<!-- After -->
<MauiIcon Include="Resources\AppIcon\appicon.png"
          ForegroundFile="Resources\AppIcon\appiconfg.png" />
```

**Rationale:** This is MAUI's intended mechanism for Android adaptive icons. The background layer (`appicon.png`) provides the solid white fill. The foreground layer (`appiconfg.png`) contains the logo with safe-zone padding so it survives circular/squircle cropping. No `Color` attribute is needed since the background is baked into the PNG.

**Alternative considered:** Using `Color="#FFFFFF"` with a transparent PNG — this was the previous approach and it fails on some Android launchers that process transparency before MAUI's color fill.

## Risks / Trade-offs

- **iOS uses both layers composited** → MAUI composites foreground onto background for iOS. Since the background is white and the foreground has proper padding, the result should match the `appicon_ios_combined.png` preview in `maui_assets/`. No risk.

- **Foreground safe-zone padding** → The `appiconfg.png` includes extra padding for Android's adaptive icon safe zone. If the padding is insufficient, the logo may be clipped on some launchers. Mitigation: The asset was designed with this in mind; preview images confirm correct rendering.
