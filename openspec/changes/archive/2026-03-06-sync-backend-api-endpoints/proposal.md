## Why

The Symfony backend has evolved (FEATURE-134 through FEATURE-140) but the MAUI client was not updated to match. The `SectionsService` calls `/api/v1/sections` which does not exist — the backend route is `/api/v1/sports`. Additionally, new fields (`voivodeship_id`, `city_id`, `condition`) and a new geography autocomplete endpoint were added server-side with no MAUI counterpart.

## What Changes

- **BREAKING**: Rename `SectionsService` API URLs from `/api/v1/sections` to `/api/v1/sports` and `/api/v1/sections/{id}/categories` to `/api/v1/sports/{id}/categories` to match the actual backend routes
- Add `VoivodeshipId` (int?) and `CityId` (int?) to the `UserAccount` model and `UpdateProfileAccountRequest` to support profile geography (FEATURE-135)
- Add `Condition` (string?) to `ListingDetail` record to match the backend listing detail response
- Add `IGeographyService.AutocompleteAsync` method and corresponding models for `GET /api/v1/geography/autocomplete` (FEATURE-136/138)
- Update `EditProfileViewModel` to expose and persist voivodeship/city selection

## Capabilities

### New Capabilities
- `geography-autocomplete`: Service method and models for the new `GET /api/v1/geography/autocomplete` endpoint returning voivodeship and city suggestions

### Modified Capabilities
- `sections-dictionary`: API URLs change from `/api/v1/sections` to `/api/v1/sports`
- `listings-api-models`: `ListingDetail` gains a `Condition` property
- `edit-profile`: Form adds voivodeship and city fields backed by `VoivodeshipId`/`CityId` on `UserAccount`

## Impact

- **Services**: `SectionsService` (URL fix), `IGeographyService` (new autocomplete method)
- **Models**: `UserAccount`, `UpdateProfileAccountRequest`, `ListingDetail`, new `AutocompleteItem`
- **Pages**: `EditProfilePage` gains geography picker fields
- **Tests**: Existing `SectionsService` tests need URL assertions updated; new tests for autocomplete and profile geography
- **JSON context**: `SportowyHubJsonContext` needs new model registrations
