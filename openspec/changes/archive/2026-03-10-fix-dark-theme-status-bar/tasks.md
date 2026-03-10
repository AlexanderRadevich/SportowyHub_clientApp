## 1. Status Bar Helper

- [x] 1.1 Create `Helpers/StatusBarHelper.cs` with a static `Apply(AppTheme theme)` method that calls `CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor()` and `StatusBar.SetStyle()` with the correct values for the given theme (light: `#FFFFFF` + `DarkContent`, dark: `#121212` + `LightContent`)

## 2. Integrate Helper into Theme-Switching Paths

- [x] 2.1 Call `StatusBarHelper.Apply()` in `HomeViewModel.ToggleTheme()` after setting `UserAppTheme`
- [x] 2.2 Call `StatusBarHelper.Apply()` in `ProfileViewModel.OnSelectedThemeIndexChanged()` after setting `UserAppTheme`
- [x] 2.3 Call `StatusBarHelper.Apply()` in `App.xaml.cs` after applying the persisted theme preference on startup

## 3. iOS Platform Configuration

- [x] 3.1 Add `UIViewControllerBasedStatusBarAppearance` = `false` to `Platforms/iOS/Info.plist`

## 4. Verification

- [x] 4.1 Build the app (`dotnet build`) and verify no compilation errors
- [x] 4.2 Run existing tests (`dotnet test`) and verify no regressions
