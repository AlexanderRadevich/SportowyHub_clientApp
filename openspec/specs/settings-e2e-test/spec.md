### Requirement: E2E test launches app and navigates to settings
The test SHALL launch the SportowyHub app on an Android emulator, wait for the home screen to load, and navigate to the Profile tab by tapping the tab bar item identified by `AutomationId` `TabProfile`.

#### Scenario: App launches and Profile tab is reachable
- **WHEN** the Appium driver creates a session with the SportowyHub app
- **THEN** the app SHALL launch, display the home screen, and the Profile tab SHALL be tappable in the bottom tab bar

#### Scenario: Profile tab displays settings section
- **WHEN** the test taps the Profile tab
- **THEN** the settings section SHALL be visible with the language picker and theme picker

### Requirement: E2E test changes language and verifies update
The test SHALL change the app language via the language picker on the Profile page and verify that UI text updates to the selected language. The test SHALL change from the default language to English (index 2) and verify that known labels display English text.

#### Scenario: Language changed to English updates settings label
- **WHEN** the test selects "English" from the language picker
- **THEN** the settings section header label SHALL display "Settings" (English) instead of the previous language text

#### Scenario: Language changed to English updates tab labels
- **WHEN** the test selects "English" from the language picker and the AppShell recreates
- **THEN** the Profile tab label SHALL display "Profile" in English

#### Scenario: Language picker retains selection after change
- **WHEN** the test selects "English" from the language picker and navigates back to the Profile tab
- **THEN** the language picker SHALL show "English" as the selected value

### Requirement: E2E test changes theme and verifies update
The test SHALL change the app theme via the theme picker on the Profile page and verify that the visual appearance changes. The test SHALL switch from the default theme to Dark (index 2).

#### Scenario: Theme changed to Dark darkens the background
- **WHEN** the test selects "Dark" from the theme picker
- **THEN** a screenshot of the Profile page SHALL show a dark background color (RGB values each below 50) in the main content area

#### Scenario: Theme changed to Dark updates icon tint colors
- **WHEN** the test selects "Dark" from the theme picker
- **THEN** a screenshot SHALL show that icon-tinted areas in the tab bar use light/white coloring (RGB values each above 200) instead of the dark Secondary color used in light theme

#### Scenario: Theme picker retains selection after change
- **WHEN** the test selects "Dark" from the theme picker and the AppShell recreates
- **THEN** the theme picker SHALL show "Dark" as the selected value

### Requirement: E2E test runs as a single ordered test fixture
The settings test SHALL execute as a single NUnit test fixture with ordered test methods. The execution order SHALL be: (1) navigate to profile, (2) change language and verify, (3) change theme and verify. Tests within the fixture SHALL share the same Appium driver session.

#### Scenario: Tests execute in defined order
- **WHEN** the test fixture runs
- **THEN** the navigation test SHALL run first, followed by the language test, followed by the theme test, using NUnit `[Order]` attributes

#### Scenario: Test fixture shares driver session
- **WHEN** all tests in the fixture execute
- **THEN** they SHALL use the same Appium `AndroidDriver` instance without restarting the app between tests
