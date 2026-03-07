## MODIFIED Requirements

### Requirement: Get sections (sports)
`GetSectionsAsync` SHALL call `GET /api/v1/sports` with optional `locale` query parameter. No auth required.

#### Scenario: Fetch all sections
- **WHEN** `GetSectionsAsync(locale: "en")` is called
- **THEN** it SHALL call `GET /api/v1/sports?locale=en` and return `SectionsResponse` with a list of sports sections

#### Scenario: Fetch sections without locale
- **WHEN** `GetSectionsAsync(locale: null)` is called
- **THEN** it SHALL call `GET /api/v1/sports` without a locale parameter

### Requirement: Get categories for section
`GetCategoriesAsync` SHALL call `GET /api/v1/sports/{id}/categories` with optional `locale` query parameter. No auth required.

#### Scenario: Fetch categories for tennis
- **WHEN** `GetCategoriesAsync(sectionId: 1, locale: "en")` is called
- **THEN** it SHALL call `GET /api/v1/sports/1/categories?locale=en` and return `CategoriesResponse`
