## Context

The backend `/api/private/profile` endpoint has been updated. Previously it returned a flat JSON object; now personal/account fields are nested inside an `account` object, and several new fields have been added. The current `UserProfile` record (13 flat fields) no longer matches the API response shape, which will break deserialization.

Current model: flat record with `FirstName`, `LastName`, `AvatarUrl`, `NotificationsEnabled`, etc.
New API shape: top-level user fields + nested `account` object + nested `oauth_linked` object.

## Goals / Non-Goals

**Goals:**
- Update `UserProfile` model to match the new nested API response structure
- Display new profile data in the UI: reputation score, balance, OAuth linked status
- Maintain all existing functionality (sign-out, loading/error states, verification badges)

**Non-Goals:**
- Editing profile fields (read-only display only)
- Loading actual avatar images (keep placeholder)
- OAuth linking/unlinking actions
- Balance top-up or transaction history
- Quiet hours or notification settings UI

## Decisions

### 1. Model structure: nested records matching API shape

Introduce two new records: `UserAccount` and `OauthLinked`. Update `UserProfile` to contain these as nested properties.

```
UserProfile (top-level)
├── Id, Email, EmailVerified, EmailVerifiedAt
├── Phone, PhoneVerified, PhoneVerifiedAt
├── TrustLevel, ReputationScore
├── OauthLinked → OauthLinked { Google }
├── LastLoginAt, LastActivityAt
├── Locale
└── Account → UserAccount
    ├── FirstName, LastName, FullName
    ├── AvatarUrl, AvatarThumbnailUrl
    ├── NotificationsEnabled
    ├── QuietHoursStart, QuietHoursEnd
    ├── Locale
    ├── BalanceGrosze, BalanceUpdatedAt
    └── (future fields land here)
```

**Rationale**: Mirrors the API 1:1. The `SnakeCaseLower` JSON naming policy handles property name mapping automatically. No custom converters needed.

**Alternative considered**: Flatten the response with `[JsonPropertyName]` attributes — rejected because it fights the API structure, complicates maintenance, and would require custom deserialization logic.

### 2. DateTime fields as `string?` not `DateTimeOffset?`

Fields like `EmailVerifiedAt`, `PhoneVerifiedAt`, `LastLoginAt`, `LastActivityAt`, `BalanceUpdatedAt` will be stored as `string?`.

**Rationale**: These are display-only timestamps. We don't perform date arithmetic on them. Keeping as strings avoids timezone parsing issues across platforms. If we need formatting later, we can parse on display.

### 3. ViewModel property access update

The ViewModel currently accesses `Profile.FirstName`, `Profile.Email`, etc. After restructuring:
- `Profile.Email` — stays (top-level)
- `Profile.FirstName` → `Profile.Account?.FirstName`
- `Profile.LastName` → `Profile.Account?.LastName`
- `Profile.TrustLevel` — stays (top-level)

`DisplayName` computation will use `Profile.Account?.FullName` first, then fall back to `FirstName`+`LastName`, then email.

### 4. UI sections layout

Keep existing sections (Header, Contact, Account) and extend Account section with new rows:

**Header**: Use `Account.FullName` → `FirstName`+`LastName` → `Email` for display name. Show reputation score below trust level.

**Contact section**: No changes (email/phone with verification badges already exist).

**Account section** — add rows:
- Reputation Score (integer display)
- Balance (format `BalanceGrosze` as currency: divide by 100, show as "X.XX zł")
- Google linked (Yes/No based on `OauthLinked.Google`)

### 5. Balance formatting

`BalanceGrosze` is an integer in grosze (1/100 PLN). Format as `"{value/100:F2} zł"` in the ViewModel as a computed property. Use invariant "zł" suffix since this is a Polish currency value, not a localized label.

### 6. New localization keys

Add to all 4 `.resx` files:
- `ProfileReputationScore` — "Reputation Score" / "Punkty reputacji" / ...
- `ProfileBalance` — "Balance" / "Saldo" / ...
- `ProfileGoogleLinked` — "Google Account" / "Konto Google" / ...
- `ProfileLinked` — "Linked" / "Połączone" / ...
- `ProfileNotLinked` — "Not linked" / "Niepołączone" / ...

## Risks / Trade-offs

- **[Breaking model change]** → All `UserProfile` property accesses must be updated (ViewModel, XAML bindings). Mitigation: small surface area — only `AccountProfileViewModel` and `AccountProfilePage.xaml` reference `UserProfile` properties directly.
- **[Null Account object]** → The API could theoretically return `account: null`. Mitigation: use null-conditional access (`Profile.Account?.FirstName`) throughout. UI bindings handle null gracefully via `TargetNullValue`.
- **[Balance display before backend supports it]** → `BalanceGrosze` may be 0 for all users initially. Mitigation: show "0.00 zł" — this is correct behavior, not an error state.
