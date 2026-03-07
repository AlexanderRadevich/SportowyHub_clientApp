## 1. Fix Sections Endpoint URLs

- [x] 1.1 Update `SectionsService.GetSectionsAsync` URLs from `/api/v1/sections` to `/api/v1/sports`
- [x] 1.2 Update `SectionsService.GetCategoriesAsync` URLs from `/api/v1/sections/{id}/categories` to `/api/v1/sports/{id}/categories`
- [x] 1.3 Update `sections-dictionary` spec in `openspec/specs/` to reflect new URLs
- [x] 1.4 Update unit tests asserting sections endpoint URLs (none existed)

## 2. Add Condition to ListingDetail

- [x] 2.1 Add `Condition` (string?) parameter to `ListingDetail` record
- [x] 2.2 Update any test factories or builders that construct `ListingDetail` instances

## 3. Add Geography Fields to Profile Models

- [x] 3.1 Add `VoivodeshipId` (int?) and `CityId` (int?) to `UserAccount` record with default values
- [x] 3.2 Add `VoivodeshipId` (int?) and `CityId` (int?) to `UpdateProfileAccountRequest` record with default values
- [x] 3.3 Update any test factories or builders that construct `UserAccount` or `UpdateProfileAccountRequest` (none existed, defaults handle compat)

## 4. Create Geography Autocomplete Service

- [x] 4.1 Create `IGeographyService` interface in `Services/Geography/`
- [x] 4.2 Create `GeographyService` implementation calling `GET /api/v1/geography/autocomplete`
- [x] 4.3 Create `GeographyAutocompleteItem` record in `Models/Api/`
- [x] 4.4 Register `GeographyAutocompleteItem` and `List<GeographyAutocompleteItem>` in `SportowyHubJsonContext`
- [x] 4.5 Register `IGeographyService`/`GeographyService` as singleton in `MauiProgram.cs`

## 5. Add Geography Pickers to Edit Profile

- [x] 5.1 Add voivodeship/city observable properties and picker item collections to `EditProfileViewModel`
- [x] 5.2 Load voivodeships from `GET /api/v1/geography/voivodeships` on page initialization
- [x] 5.3 Load cities from `GET /api/v1/geography/cities?voivodeship_id={id}` when voivodeship changes
- [x] 5.4 Pre-populate voivodeship/city pickers from `Account.VoivodeshipId`/`Account.CityId`
- [x] 5.5 Include `VoivodeshipId` and `CityId` in the save command's `UpdateProfileAccountRequest`
- [x] 5.6 Add Voivodeship and City `Picker` controls to `EditProfilePage.xaml`
- [x] 5.7 Add localization strings `EditProfileVoivodeship` and `EditProfileCity` across all 4 languages (pl, en, uk, ru)

## 6. Verification

- [x] 6.1 Run `dotnet build` and fix any compiler errors
- [x] 6.2 Run `dotnet test` and fix any failing tests
- [x] 6.3 Run `dotnet format --verify-no-changes` and fix formatting issues
