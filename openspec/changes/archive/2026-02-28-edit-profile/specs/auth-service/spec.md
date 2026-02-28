## ADDED Requirements

### Requirement: UpdateProfileAsync on auth service
`IAuthService` SHALL expose a `Task<UserProfile?> UpdateProfileAsync(UpdateProfileRequest request)` method. `AuthService` SHALL implement it by reading the access token from SecureStorage and calling `PUT /api/private/profile` with Bearer authorization and the request body. The method SHALL return the updated `UserProfile` on success and let exceptions propagate to the caller.

#### Scenario: UpdateProfileAsync returns updated profile on success
- **WHEN** `UpdateProfileAsync` is called with valid data and the user has a valid token
- **THEN** the method SHALL return the updated `UserProfile` from the API response

#### Scenario: UpdateProfileAsync returns null when no token exists
- **WHEN** `UpdateProfileAsync` is called and no token is stored
- **THEN** the method SHALL return `null`

#### Scenario: UpdateProfileAsync propagates API errors
- **WHEN** the API returns a 422 validation error
- **THEN** the exception SHALL propagate to the caller with the error response body for field-level error parsing
