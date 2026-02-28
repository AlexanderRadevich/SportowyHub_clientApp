## ADDED Requirements

### Requirement: UserAccount nested model
The app SHALL define a `UserAccount` record with properties: `FirstName` (string?), `LastName` (string?), `FullName` (string?), `AvatarUrl` (string?), `AvatarThumbnailUrl` (string?), `NotificationsEnabled` (bool), `QuietHoursStart` (string?), `QuietHoursEnd` (string?), `Locale` (string?), `BalanceGrosze` (int), `BalanceUpdatedAt` (string?). The type SHALL be registered in `SportowyHubJsonContext` for source-generated serialization with `SnakeCaseLower` naming policy.

#### Scenario: UserAccount deserializes from nested account object
- **WHEN** the API returns a profile response with `"account": {"first_name": "John", "last_name": "Doe", "full_name": "John Doe", "balance_grosze": 1500, "notifications_enabled": true}`
- **THEN** the `UserAccount` record SHALL contain FirstName="John", LastName="Doe", FullName="John Doe", BalanceGrosze=1500, NotificationsEnabled=true

#### Scenario: UserAccount handles all-null optional fields
- **WHEN** the API returns an account object with all nullable fields as null
- **THEN** the corresponding C# properties SHALL be `null`, and `BalanceGrosze` SHALL be 0, `NotificationsEnabled` SHALL deserialize to its actual value

### Requirement: OauthLinked nested model
The app SHALL define an `OauthLinked` record with property `Google` (bool). The type SHALL be registered in `SportowyHubJsonContext` for source-generated serialization with `SnakeCaseLower` naming policy.

#### Scenario: OauthLinked deserializes from API response
- **WHEN** the API returns `"oauth_linked": {"google": true}`
- **THEN** the `OauthLinked` record SHALL contain Google=true

#### Scenario: OauthLinked with no providers linked
- **WHEN** the API returns `"oauth_linked": {"google": false}`
- **THEN** the `OauthLinked` record SHALL contain Google=false

### Requirement: JSON context registrations for new profile models
`SportowyHubJsonContext` SHALL include `[JsonSerializable]` attributes for `UserAccount` and `OauthLinked`.

#### Scenario: New models are serializable via source generation
- **WHEN** `UserAccount` or `OauthLinked` is deserialized via the source-generated JSON context
- **THEN** the operation SHALL succeed without runtime reflection

## MODIFIED Requirements

### Requirement: UserProfile API model
The app SHALL define a `UserProfile` record matching the `GET /api/private/profile` nested response. The record SHALL include top-level fields: `Id` (int), `Email` (string), `EmailVerified` (bool), `EmailVerifiedAt` (string?), `Phone` (string?), `PhoneVerified` (bool), `PhoneVerifiedAt` (string?), `TrustLevel` (string), `ReputationScore` (int), `OauthLinked` (OauthLinked?), `LastLoginAt` (string?), `LastActivityAt` (string?), `Locale` (string?). The record SHALL include a nested `Account` property of type `UserAccount?`. All nullable JSON fields SHALL map to nullable C# types. The type SHALL be registered in `SportowyHubJsonContext` for source-generated serialization with `SnakeCaseLower` naming policy.

#### Scenario: UserProfile deserializes full nested API response
- **WHEN** the API returns a complete `/api/private/profile` JSON payload with nested `account` and `oauth_linked` objects
- **THEN** the `UserProfile` record SHALL contain all top-level fields and nested `Account` and `OauthLinked` objects with correct values

#### Scenario: UserProfile new top-level fields deserialized
- **WHEN** the API returns `"reputation_score": 5, "email_verified_at": "2026-01-15T10:00:00+01:00", "last_login_at": null`
- **THEN** `ReputationScore` SHALL be 5, `EmailVerifiedAt` SHALL be "2026-01-15T10:00:00+01:00", `LastLoginAt` SHALL be null

#### Scenario: Nullable nested objects handled correctly
- **WHEN** the API returns `"account": null` or `"oauth_linked": null`
- **THEN** `Account` SHALL be null and `OauthLinked` SHALL be null respectively
