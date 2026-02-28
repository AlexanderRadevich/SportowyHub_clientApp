## Context

The Account Profile page displays user data in read-only mode. The backend `PUT /api/private/profile` endpoint accepts partial updates with a nested `account` object. The app already has form patterns (Login, Register) with validation, error handling, and loading states that we can follow.

Key constraint: the current `IRequestProvider.PutAsync<TResult>` uses the same type for request and response. The edit profile request body (`UpdateProfileRequest`) differs from the response (`UserProfile`), so we need a new overload.

## Goals / Non-Goals

**Goals:**
- Allow users to edit: first name, last name, phone, notifications toggle, quiet hours
- Validate inputs client-side before submission
- Show field-level errors from API validation responses
- Pre-populate form with current profile data
- Navigate back and refresh profile on successful save

**Non-Goals:**
- Avatar upload (separate endpoint, separate change)
- Locale editing (handled by existing language picker in settings)
- Email editing (not supported by API)
- Quiet hours time picker UI (use plain text Entry with HH:mm format for now)

## Decisions

### 1. Add `PutAsync<TRequest, TResponse>` overload to IRequestProvider

The current `PutAsync<TResult>` assumes request and response share the same type. Profile edit sends `UpdateProfileRequest` but receives `UserProfile`. Add a two-type-parameter overload matching the existing `PostAsync<TRequest, TResponse>` signature.

**Alternative considered**: Serialize manually and use a raw HTTP call — rejected because it bypasses the established `RequestProvider` pattern and error handling.

### 2. Request DTO structure: flat with nested account

Match the API body exactly:

```
UpdateProfileRequest
├── Phone (string?)
└── Account (UpdateProfileAccountRequest?)
    ├── FirstName (string?)
    ├── LastName (string?)
    ├── NotificationsEnabled (bool)
    ├── QuietHoursStart (string?)
    └── QuietHoursEnd (string?)
```

Omit `locale` (top-level) and `account.locale` — locale is managed by the existing language picker setting. Omit `account.avatar_url` — avatar upload is a separate feature.

**Alternative considered**: Single flat request DTO — rejected because the API expects the nested structure.

### 3. Pre-populate form from current profile

`EditProfilePage` receives the current `UserProfile` as a navigation query parameter (serialized). The ViewModel initializes editable fields from it. This avoids a redundant API call.

**Alternative considered**: Re-fetch profile on edit page load — rejected because profile was just loaded on the previous page.

### 4. Navigation: pass profile via ViewModel init

Use `IQueryAttributable` on the ViewModel. The AccountProfilePage navigates with `Shell.Current.GoToAsync("edit-profile")` and the ViewModel receives the current Profile object. Since `UserProfile` is complex, store it temporarily in a static/shared property rather than serializing to query string.

**Alternative considered**: Query string serialization — rejected because records with nested objects are cumbersome to serialize into URI parameters.

### 5. Form layout follows LoginPage pattern

Use the same `ScrollView` → `VerticalStackLayout` → `Label` + `Border` + `Entry` pattern. Add a `Switch` for notifications toggle and plain `Entry` fields for quiet hours (HH:mm format).

### 6. Save returns updated profile, navigate back

On successful PUT, the API returns the updated `UserProfile`. Navigate back with `..` and the AccountProfilePage will re-fetch on `OnAppearing`. Show a success toast.

### 7. Error handling follows AuthService pattern

Parse `ApiError` with field violations. Map API field names (snake_case) to form fields. Show field-specific errors below each input and a general error for non-field errors.

## Risks / Trade-offs

- **[PutAsync overload]** → Adding a method to `IRequestProvider` is a cross-cutting change. Mitigation: minimal change, follows existing `PostAsync` pattern exactly.
- **[Passing profile between pages]** → Static property is not ideal for complex navigation scenarios. Mitigation: acceptable for single-level push/pop navigation; clear the reference after consumption.
- **[Quiet hours as text entry]** → Users may enter invalid time formats. Mitigation: validate HH:mm pattern client-side. A proper TimePicker can be added later.
