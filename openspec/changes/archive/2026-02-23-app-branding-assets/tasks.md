## 1. App Icon

- [x] 1.1 Copy `maui_assets/Resources/AppIcon/appicon.png` to `Resources/AppIcon/appicon.png`
- [x] 1.2 Remove `Resources/AppIcon/appicon.svg` and `Resources/AppIcon/appiconfg.svg`
- [x] 1.3 Update `.csproj` `MauiIcon` entry: replace SVG reference with `<MauiIcon Include="Resources\AppIcon\appicon.png" Color="#FFFFFF" />`

## 2. Splash Screen

- [x] 2.1 Copy `maui_assets/Resources/Splash/splash.png` to `Resources/Splash/splash.png`
- [x] 2.2 Remove `Resources/Splash/splash.svg`
- [x] 2.3 Update `.csproj` `MauiSplashScreen` entry: replace SVG reference with `<MauiSplashScreen Include="Resources\Splash\splash.png" Color="#FFFFFF" BaseSize="620,157" />`

## 3. In-App Logo & Icon Images

- [x] 3.1 Copy `maui_assets/Resources/Images/logo_full.png` and height variants (`logo_full_40h.png`, `logo_full_60h.png`, `logo_full_80h.png`, `logo_full_120h.png`) to `Resources/Images/`
- [x] 3.2 Copy `maui_assets/Resources/Images/icon_32.png` through `icon_512.png` to `Resources/Images/`

## 4. Template Asset Cleanup

- [x] 4.1 Remove `Resources/Images/dotnet_bot.png`
- [x] 4.2 Remove the `<MauiImage Update="Resources\Images\dotnet_bot.png" ...>` entry from `.csproj`

## 5. Profile Page Update

- [x] 5.1 Update `Views/Profile/ProfilePage.xaml`: change `Image Source="dotnet_bot.png"` to `Source="logo_full.png"` and adjust `HeightRequest` appropriately for the logo

## 6. Verify

- [x] 6.1 Build the project and verify it compiles without errors
