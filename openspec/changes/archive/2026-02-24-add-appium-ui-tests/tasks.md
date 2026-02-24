## 1. Add AutomationId attributes to existing XAML

- [x] 1.1 Add `AutomationId="TabHome"`, `AutomationId="TabSearch"`, `AutomationId="TabFavorites"`, `AutomationId="TabProfile"` to the four `ShellContent` elements in `AppShell.xaml`
- [x] 1.2 Add `AutomationId="SettingsLabel"` to the settings section header label in `Views/Profile/ProfilePage.xaml`
- [x] 1.3 Add `AutomationId="LanguageLabel"` and `AutomationId="LanguagePicker"` to the language row label and Picker in `Views/Profile/ProfilePage.xaml`
- [x] 1.4 Add `AutomationId="ThemeLabel"` and `AutomationId="ThemePicker"` to the theme row label and Picker in `Views/Profile/ProfilePage.xaml`

## 2. Create test project and configuration

- [x] 2.1 Create `SportowyHub.UITests/SportowyHub.UITests.csproj` targeting `net10.0` with NUnit 4.x, NUnit3TestAdapter, Microsoft.NET.Test.Sdk, and Appium.WebDriver 8.x package references
- [x] 2.2 Create `SportowyHub.UITests/Config/TestConfig.cs` with Appium desired capabilities (platformName Android, automationName UiAutomator2, appPackage, appActivity, Appium server URL)

## 3. Create test helpers

- [x] 3.1 Create `SportowyHub.UITests/Helpers/AppiumSetup.cs` base test class with `[OneTimeSetUp]` to create `AndroidDriver` and `[OneTimeTearDown]` to dispose it
- [x] 3.2 Create `SportowyHub.UITests/Helpers/ScreenshotHelper.cs` with methods to capture a screenshot from the driver and sample pixel RGB color at given coordinates

## 4. Create page objects

- [x] 4.1 Create `SportowyHub.UITests/Pages/AppShellPage.cs` with method to navigate to a tab by AutomationId (e.g., `NavigateToProfile()`) using `MobileBy.Id` and explicit waits
- [x] 4.2 Create `SportowyHub.UITests/Pages/ProfilePage.cs` with methods to select a language by index, select a theme by index, and read current picker values and label text, using `MobileBy.Id` with AutomationId locators and explicit waits

## 5. Create settings E2E test

- [x] 5.1 Create `SportowyHub.UITests/Tests/SettingsTests.cs` test fixture inheriting from `AppiumSetup`, with `[Order]` attributes on three test methods
- [x] 5.2 Implement test method 1: navigate to Profile tab and assert settings section is visible (language picker and theme picker present)
- [x] 5.3 Implement test method 2: select English from language picker, wait for AppShell recreation, navigate back to Profile tab, assert settings label shows "Settings" and language picker shows "English"
- [x] 5.4 Implement test method 3: select Dark from theme picker, wait for AppShell recreation, capture screenshot, assert background pixel color is dark (RGB < 50), assert tab bar icon area pixel color is light (RGB > 200), navigate back to Profile and assert theme picker shows "Dark"

## 6. Verify test project builds

- [x] 6.1 Run `dotnet build` on the test project and confirm it compiles with no errors
