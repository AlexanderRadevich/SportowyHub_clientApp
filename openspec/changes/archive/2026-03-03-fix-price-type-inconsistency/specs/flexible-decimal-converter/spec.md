## ADDED Requirements

### Requirement: FlexibleDecimalConverter deserializes JSON numbers to decimal
The `FlexibleDecimalConverter` SHALL deserialize JSON number tokens into `decimal?` values.

#### Scenario: JSON number value
- **WHEN** the JSON contains a numeric price value (e.g., `150.0`)
- **THEN** the converter SHALL return the corresponding `decimal?` value (`150.0m`)

#### Scenario: JSON null value
- **WHEN** the JSON contains a `null` price value
- **THEN** the converter SHALL return `null`

### Requirement: FlexibleDecimalConverter deserializes JSON strings to decimal
The `FlexibleDecimalConverter` SHALL deserialize JSON string tokens containing numeric values into `decimal?` using `InvariantCulture` parsing.

#### Scenario: JSON string numeric value
- **WHEN** the JSON contains a string price value (e.g., `"150.00"`)
- **THEN** the converter SHALL parse it to the corresponding `decimal?` value (`150.00m`)

#### Scenario: JSON string non-numeric value
- **WHEN** the JSON contains a string price value that cannot be parsed as decimal (e.g., `"N/A"`)
- **THEN** the converter SHALL return `null`

### Requirement: FlexibleDecimalConverter serializes decimal to JSON number
The `FlexibleDecimalConverter` SHALL serialize `decimal?` values as JSON number tokens, and `null` as JSON null.

#### Scenario: Serialize non-null decimal
- **WHEN** the `decimal?` value is `150.00m`
- **THEN** the converter SHALL write a JSON number token

#### Scenario: Serialize null decimal
- **WHEN** the `decimal?` value is `null`
- **THEN** the converter SHALL write a JSON null token

### Requirement: FlexibleDecimalConverter is compatible with source-generated JSON
The converter SHALL work with `System.Text.Json` source generation (`JsonSerializerContext`) when applied via `[JsonConverter]` attribute on record properties.

#### Scenario: Source-generated deserialization
- **WHEN** a record property has `[property: JsonConverter(typeof(FlexibleDecimalConverter))]`
- **THEN** the source-generated serializer SHALL use the converter for that property
