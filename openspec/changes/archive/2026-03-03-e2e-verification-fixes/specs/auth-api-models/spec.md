## MODIFIED Requirements

### Requirement: UserProfile model
The app SHALL define a `UserProfile` record with properties: `Id` (int), `Email` (string), `EmailVerified` (bool), `EmailVerifiedAt` (string?), `Phone` (string?), `PhoneVerified` (bool), `PhoneVerifiedAt` (string?), `TrustLevel` (string), `ReputationScore` (decimal, with `[JsonConverter(typeof(FlexibleDecimalConverter))]`), `OauthLinked` (OauthLinked?), `LastLoginAt` (string?), `LastActivityAt` (string?), `Account` (UserAccount?). The type SHALL NOT have a top-level `Locale` property — locale is accessible only via `Account.Locale`.

#### Scenario: UserProfile deserializes from GET /api/private/profile
- **WHEN** the API returns a profile response with `"reputation_score": 4.75`
- **THEN** `UserProfile.ReputationScore` SHALL be `4.75m`

#### Scenario: UserProfile deserializes integer reputation score
- **WHEN** the API returns a profile response with `"reputation_score": 5`
- **THEN** `UserProfile.ReputationScore` SHALL be `5m`

#### Scenario: UserProfile has no top-level Locale property
- **WHEN** the API returns a profile response with `"account": {"locale": "pl"}`
- **THEN** locale SHALL be accessible only via `UserProfile.Account.Locale`, and no top-level `Locale` property SHALL exist

## REMOVED Requirements

### Requirement: UserProfile top-level Locale property
**Reason**: The backend GET `/api/private/profile` response nests `locale` inside the `account` object, not at the root level. The top-level `Locale` property always deserialized as `null`, causing silent data loss.
**Migration**: Access locale via `UserProfile.Account?.Locale` instead of `UserProfile.Locale`.
