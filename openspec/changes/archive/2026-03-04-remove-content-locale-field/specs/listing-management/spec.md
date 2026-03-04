## MODIFIED Requirements

### Requirement: CreateListingRequest model
The app SHALL define a `CreateListingRequest` record with: `CategoryId` (int), `Title` (string), `Description` (string), `Price` (decimal?), `Currency` (string?), `CityId` (int), `VoivodeshipId` (int), `LocationLatitude` (double?), `LocationLongitude` (double?), `Attributes` (Dictionary<string, string>?), `ContentLocale` (string?). Registered in `SportowyHubJsonContext`. The `ContentLocale` value SHALL be populated by the ViewModel from `ILocaleService`, not from user input.

#### Scenario: Serialize create request
- **WHEN** a `CreateListingRequest` is serialized
- **THEN** all fields SHALL use snake_case naming (e.g., `category_id`, `city_id`, `voivodeship_id`)

#### Scenario: ContentLocale is set from app locale
- **WHEN** `CreateListingAsync` is called
- **THEN** the request's `ContentLocale` SHALL contain the current app locale string (one of `pl`, `en`, `uk`, `ru`) as returned by `ILocaleService.GetLocaleInfoAsync().Locale`

### Requirement: UpdateListingRequest model
The app SHALL define an `UpdateListingRequest` record with the same fields as `CreateListingRequest`, all nullable. Registered in `SportowyHubJsonContext`. The `ContentLocale` value SHALL be populated by the ViewModel from `ILocaleService`, not from user input.

#### Scenario: Serialize partial update
- **WHEN** an `UpdateListingRequest` with only Title="New Title" is serialized
- **THEN** only non-null fields SHALL be included in the JSON

#### Scenario: ContentLocale is set from app locale on update
- **WHEN** `UpdateListingAsync` is called
- **THEN** the request's `ContentLocale` SHALL contain the current app locale string as returned by `ILocaleService`
