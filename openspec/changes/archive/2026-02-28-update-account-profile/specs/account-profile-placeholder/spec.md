## MODIFIED Requirements

### Requirement: Profile header display
The Account Profile page SHALL display a header area at the top containing: an avatar circle placeholder (person icon), the user's display name, and a trust level indicator with reputation score.

#### Scenario: Header shows full name from Account.FullName when available
- **WHEN** the profile has `Account.FullName` set (e.g., "John Doe")
- **THEN** the header SHALL display `Account.FullName` as the primary text and the email below it

#### Scenario: Header falls back to FirstName/LastName when FullName is null
- **WHEN** the profile has `Account.FullName` as null but `Account.FirstName` and/or `Account.LastName` set
- **THEN** the header SHALL display the combined FirstName+LastName as the primary text and the email below it

#### Scenario: Header falls back to email when all names are null
- **WHEN** the profile has `Account.FullName`, `Account.FirstName`, and `Account.LastName` all null (or Account is null)
- **THEN** the header SHALL display the email as the primary text

#### Scenario: Header shows trust level and reputation score
- **WHEN** the profile is displayed with TrustLevel="TL1" and ReputationScore=5
- **THEN** the trust level and reputation score SHALL both be visible in the header area, formatted as "TL1 · 5 rep"

#### Scenario: Header shows avatar placeholder
- **WHEN** the profile is displayed (regardless of `Account.AvatarUrl` value)
- **THEN** a circle placeholder with a person icon SHALL be displayed (avatar image loading is out of scope)

### Requirement: Account section display
The Account Profile page SHALL display an "Account" section with rows for trust level, reputation score, balance, and Google account link status.

#### Scenario: Trust level displayed
- **WHEN** the profile has `TrustLevel` = "TL1"
- **THEN** the Account section SHALL display a "Trust Level" row with value "TL1"

#### Scenario: Reputation score displayed
- **WHEN** the profile has `ReputationScore` = 5
- **THEN** the Account section SHALL display a localized "Reputation Score" row with value "5"

#### Scenario: Balance displayed as currency
- **WHEN** the profile has `Account.BalanceGrosze` = 1500
- **THEN** the Account section SHALL display a localized "Balance" row with value "15.00 zł"

#### Scenario: Zero balance displayed
- **WHEN** the profile has `Account.BalanceGrosze` = 0
- **THEN** the Account section SHALL display a localized "Balance" row with value "0.00 zł"

#### Scenario: Balance row hidden when Account is null
- **WHEN** the profile has `Account` = null
- **THEN** the Balance row SHALL display "0.00 zł" (default)

#### Scenario: Google linked status displayed as linked
- **WHEN** the profile has `OauthLinked.Google` = true
- **THEN** the Account section SHALL display a "Google Account" row with localized "Linked" text in `Success` color

#### Scenario: Google linked status displayed as not linked
- **WHEN** the profile has `OauthLinked.Google` = false (or `OauthLinked` is null)
- **THEN** the Account section SHALL display a "Google Account" row with localized "Not linked" text in `TextSecondary` color

### Requirement: AccountProfileViewModel nested property access
The `AccountProfileViewModel` SHALL access profile properties through the nested structure. `DisplayName` SHALL use `Profile.Account?.FullName` first, then fall back to combined `Profile.Account?.FirstName`/`Profile.Account?.LastName`, then `Profile.Email`. The ViewModel SHALL expose computed properties for formatted balance and reputation display.

#### Scenario: DisplayName uses FullName when available
- **WHEN** `Profile.Account.FullName` = "John Doe"
- **THEN** `DisplayName` SHALL return "John Doe"

#### Scenario: DisplayName falls back to FirstName+LastName
- **WHEN** `Profile.Account.FullName` is null and `FirstName` = "John", `LastName` = "Doe"
- **THEN** `DisplayName` SHALL return "John Doe"

#### Scenario: DisplayName falls back to Email
- **WHEN** `Profile.Account` is null or all name fields are null
- **THEN** `DisplayName` SHALL return `Profile.Email`

#### Scenario: FormattedBalance computes currency string
- **WHEN** `Profile.Account.BalanceGrosze` = 1500
- **THEN** `FormattedBalance` SHALL return "15.00 zł"

#### Scenario: TrustInfo combines trust level and reputation
- **WHEN** `Profile.TrustLevel` = "TL1" and `Profile.ReputationScore` = 5
- **THEN** the header subtitle SHALL display "TL1 · 5 rep"

### Requirement: Profile localization strings
The app SHALL define localized strings for all profile labels and status texts across all 4 languages (pl, en, uk, ru). Required keys: `ProfileContact`, `ProfileEmail`, `ProfilePhone`, `ProfileAccount`, `ProfileVerified`, `ProfileNotVerified`, `ProfileNotSet`, `ProfileTrustLevel`, `ProfileLoadError`, `ProfileReputationScore`, `ProfileBalance`, `ProfileGoogleLinked`, `ProfileLinked`, `ProfileNotLinked`.

#### Scenario: All profile labels are localized
- **WHEN** the Account Profile page is displayed in any supported language
- **THEN** all section headers, field labels, and status texts SHALL use localized strings from `AppResources`

#### Scenario: New localization keys exist in all languages
- **WHEN** the app is built
- **THEN** `ProfileReputationScore`, `ProfileBalance`, `ProfileGoogleLinked`, `ProfileLinked`, `ProfileNotLinked` SHALL exist in all 4 `.resx` files (en, pl, uk, ru)
