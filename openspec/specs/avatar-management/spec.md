## Purpose

Defines the service interface and model for uploading and deleting the user's profile avatar via multipart HTTP requests to the private API.

## Requirements

### Requirement: Upload avatar
`IAuthService` SHALL expose `Task<AvatarResponse?> UploadAvatarAsync(Stream imageStream, string fileName, CancellationToken ct)`. It SHALL POST multipart/form-data to `/api/private/profile/avatar` with the image as the `file` field. Supported types: JPEG, PNG, WebP. Max size: 2MB.

#### Scenario: Upload JPEG avatar
- **WHEN** `UploadAvatarAsync` is called with a JPEG image stream
- **THEN** the service SHALL POST multipart/form-data with Bearer auth and return `AvatarResponse` containing `AvatarUrl` and `AvatarThumbnailUrl`

#### Scenario: Upload too-large file
- **WHEN** `UploadAvatarAsync` is called with a file exceeding 2MB
- **THEN** the backend SHALL return 422 and the exception SHALL propagate to the caller

### Requirement: Delete avatar
`IAuthService` SHALL expose `Task DeleteAvatarAsync(CancellationToken ct)`. It SHALL send `DELETE /api/private/profile/avatar` with Bearer auth.

#### Scenario: Delete existing avatar
- **WHEN** `DeleteAvatarAsync` is called
- **THEN** the service SHALL DELETE `/api/private/profile/avatar` with Bearer auth

### Requirement: AvatarResponse model
The app SHALL define an `AvatarResponse` record with `AvatarUrl` (string?) and `AvatarThumbnailUrl` (string?). It SHALL be registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize avatar upload response
- **WHEN** the API returns `{"avatar_url":"https://...","avatar_thumbnail_url":"https://..."}`
- **THEN** it SHALL deserialize to `AvatarResponse` with both URLs populated

### Requirement: PostMultipartAsync on RequestProvider
`IRequestProvider` SHALL expose a `PostMultipartAsync<TResponse>(string uri, MultipartFormDataContent content, string token)` method for file uploads. It SHALL set Bearer auth and return the deserialized response.

#### Scenario: Multipart POST with auth
- **WHEN** `PostMultipartAsync<AvatarResponse>` is called with content and a token
- **THEN** the request SHALL use POST with `multipart/form-data` content type and Bearer authorization
