## MODIFIED Requirements

### Requirement: Settings section with picker rows
The Settings section SHALL display a section header with the localized text `ProfileSettingsSection`. Below the header, the section SHALL contain two rows: "Language" (`SettingsLanguage`) and "Theme" (`SettingsTheme`). Each row SHALL display the localized setting label on the left and the current selection value on the right. The settings section header label SHALL have `AutomationId="SettingsLabel"`. The language setting label SHALL have `AutomationId="LanguageLabel"`. The theme setting label SHALL have `AutomationId="ThemeLabel"`. The language Picker SHALL have `AutomationId="LanguagePicker"`. The theme Picker SHALL have `AutomationId="ThemePicker"`.

#### Scenario: Settings section displays Language and Theme rows
- **WHEN** the profile hub is displayed
- **THEN** the Settings section SHALL show "Language" and "Theme" rows with their current values displayed

#### Scenario: Settings elements are identifiable by AutomationId
- **WHEN** the profile hub is inspected via Appium or accessibility tools
- **THEN** the settings section label SHALL be locatable by `AutomationId` `SettingsLabel`, the language picker by `LanguagePicker`, the theme picker by `ThemePicker`, the language label by `LanguageLabel`, and the theme label by `ThemeLabel`
