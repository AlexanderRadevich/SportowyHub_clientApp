## Context

SportowyHub is a .NET MAUI app targeting Android (net10.0-android). It has zero test infrastructure today. The Profile tab serves as the settings screen, with Picker controls for language (5 options: System/Polski/English/Українська/Русский) and theme (3 options: System/Light/Dark). Changing either setting recreates the entire AppShell to refresh bindings. No UI elements currently carry `AutomationId` attributes.

## Goals / Non-Goals

**Goals:**
- Stand up a working Appium test project that can run against the Android app on an emulator
- Cover the settings flow end-to-end: navigate to Profile tab → change language → verify → change theme → verify icon colors
- Establish reusable patterns (base class, page objects, helpers) so future tests are easy to add

**Non-Goals:**
- iOS testing (Android only for now)
- CI/CD pipeline integration (local execution only)
- Full visual regression suite or baseline image management
- Testing auth flows (login/register) — only settings in this change

## Decisions

### 1. Appium 3 + UIAutomator2 driver

Use Appium server 3.x with the UIAutomator2 driver.

- UIAutomator2 is the standard driver used in all Microsoft MAUI Appium samples and community tutorials
- It is a black-box driver that treats the MAUI app as a normal Android app — no framework-specific hooks needed
- Espresso was considered but rejected because it requires compiling into the app itself, adding complexity with no benefit for our use case

### 2. NUnit test framework

Use NUnit 4.x as the test runner.

- NUnit's `[OneTimeSetUp]` / `[OneTimeTearDown]` fit naturally for Appium driver lifecycle (create session once per fixture, not per test)
- The official Microsoft MAUI + Appium sample uses NUnit, giving us well-documented patterns to follow
- xUnit was considered but requires `IAsyncLifetime` boilerplate for setup/teardown that NUnit handles with attributes

### 3. Appium.WebDriver 8.x NuGet package

Use `Appium.WebDriver` version 8.1.0 (latest stable).

- This is the only official Appium .NET client
- Version 8.x is compatible with Appium server 3.x and uses Selenium 4.36+
- Older versions (5.x) from the original Microsoft sample are outdated

### 4. Element location via AutomationId → MobileBy.Id

Add `AutomationId` attributes to MAUI XAML elements. In Appium tests, locate them with `MobileBy.Id("TheAutomationId")`.

- On Android, MAUI maps `AutomationId` to the view's `resource-id`
- UIAutomator2 resolves short-form IDs (just the AutomationId value) without needing the full `com.companyname.sportowyhub:id/` prefix
- This is the most stable locator strategy — survives text changes and layout refactors

### 5. Theme verification via screenshot pixel sampling

Appium cannot read native element colors (no `GetCssValue` for native views). Two-pronged approach:

- **Primary**: Take a screenshot after theme change, decode it as a bitmap, sample pixel colors at known regions (e.g., page background center, icon areas) to verify light vs dark
- **Secondary**: Verify the picker selection index persisted correctly as an indirect check

Baseline image comparison was considered but rejected — too brittle for early-stage testing and requires OpenCV on the Appium server.

### 6. Page Object Model

Encapsulate element locators and UI actions in page classes (`ProfilePage`, `AppShellPage`). Tests call high-level methods like `profilePage.SelectLanguage("English")` rather than raw driver calls.

- Isolates tests from locator changes
- Makes tests readable and maintainable

### 7. Project structure

```
SportowyHub_clientApp/
├── SportowyHub.csproj              # Existing MAUI app
├── SportowyHub.UITests/
│   ├── SportowyHub.UITests.csproj  # net10.0, NUnit + Appium.WebDriver
│   ├── Config/
│   │   └── TestConfig.cs           # App path, Appium URL, capabilities
│   ├── Helpers/
│   │   ├── AppiumSetup.cs          # Base test class (driver init/teardown)
│   │   └── ScreenshotHelper.cs     # Screenshot capture + pixel sampling
│   ├── Pages/
│   │   ├── AppShellPage.cs         # Tab navigation
│   │   └── ProfilePage.cs         # Language/theme picker interactions
│   └── Tests/
│       └── SettingsTests.cs        # The E2E settings test
```

Single test project targeting plain `net10.0` (not a MAUI project). No solution file changes needed — can be built and run independently with `dotnet test`.

### 8. AutomationId additions to existing XAML

Minimal set of IDs needed for the test:

| Element | AutomationId | File |
|---------|-------------|------|
| Profile tab | `TabProfile` | AppShell.xaml |
| Language picker | `LanguagePicker` | ProfilePage.xaml |
| Theme picker | `ThemePicker` | ProfilePage.xaml |
| Settings section label | `SettingsLabel` | ProfilePage.xaml |
| Language label | `LanguageLabel` | ProfilePage.xaml |
| Theme label | `ThemeLabel` | ProfilePage.xaml |

These are additive-only changes — no behavioral impact on the app.

## Risks / Trade-offs

- **AppShell recreation on settings change** → After changing language or theme, the app rebuilds the entire shell. The Appium session may need a short wait or element re-query after the transition. Mitigation: add explicit waits (WebDriverWait) after picker interactions.
- **Pixel sampling is resolution-dependent** → Screenshots vary by emulator screen density. Mitigation: use relative coordinates (percentage of screen) and assert color ranges rather than exact values.
- **No Appium server auto-start** → Tests assume Appium is already running. Mitigation: document the setup steps clearly; auto-start can be added later as an enhancement.
- **Android-only** → Tests won't cover iOS. Acceptable per non-goals; the page object pattern makes future iOS expansion straightforward.
- **Picker interaction on Android** → MAUI Pickers render as native Android spinners/dialogs. UIAutomator2 can interact with them, but the exact interaction pattern (click picker → click item in dropdown) may need tuning per Android version. Mitigation: test on a consistent emulator API level (API 35).
