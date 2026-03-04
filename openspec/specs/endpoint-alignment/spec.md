# Endpoint Alignment

## Purpose

Ensures all MAUI service endpoint URI strings match their backend route definitions exactly, with no trailing slashes on mutation endpoints.

## Requirements

### Requirement: Listing create endpoint has no trailing slash
`ListingManagementService.CreateListingAsync` SHALL send `POST /api/private/listings` (no trailing slash).

#### Scenario: Create listing request uses correct URL
- **GIVEN** the user submits a create listing form
- **WHEN** `CreateListingAsync` builds the HTTP request
- **THEN** the request URI is `/api/private/listings` with no trailing slash

#### Scenario: No 308 redirect occurs on listing create
- **GIVEN** the backend route is registered at `/api/private/listings` without a trailing slash
- **WHEN** the client sends `POST /api/private/listings`
- **THEN** the server responds with `201 Created` without any redirect

### Requirement: Conversation create endpoint has no trailing slash
`MessagingService.CreateConversationAsync` SHALL send `POST /api/private/conversations` (no trailing slash).

#### Scenario: Create conversation request uses correct URL
- **GIVEN** the user initiates a new conversation on a listing
- **WHEN** `CreateConversationAsync` builds the HTTP request
- **THEN** the request URI is `/api/private/conversations` with no trailing slash

#### Scenario: No 308 redirect occurs on conversation create
- **GIVEN** the backend route is registered at `/api/private/conversations` without a trailing slash
- **WHEN** the client sends `POST /api/private/conversations`
- **THEN** the server responds with `201 Created` without any redirect

### Requirement: Media upload endpoint has no trailing slash
`MediaService.UploadAsync` SHALL send `POST /api/private/media` (no trailing slash).

#### Scenario: Media upload request uses correct URL
- **GIVEN** the user uploads an image to a listing
- **WHEN** `UploadAsync` builds the multipart HTTP request
- **THEN** the request URI is `/api/private/media` with no trailing slash

#### Scenario: No 308 redirect occurs on media upload
- **GIVEN** the backend route is registered at `/api/private/media` without a trailing slash
- **WHEN** the client sends `POST /api/private/media`
- **THEN** the server responds with `201 Created` without any redirect

### Requirement: All remaining private and public endpoints have no trailing slashes
All other service endpoint URIs (auth, sections, favorites, search, DSAR, phone verification, theme, locale) SHALL match their backend route definitions exactly and contain no trailing slashes.

#### Scenario: Verified endpoint catalogue matches backend
- **GIVEN** the full list of backend route definitions in Symfony controllers
- **WHEN** each MAUI service URI string is compared against the corresponding backend `#[Route]` attribute
- **THEN** every URI matches exactly with no trailing slash discrepancy on any non-GET mutation endpoint
