## Purpose

Defines the service interface and models for the in-app messaging system, allowing users to create conversations about listings and exchange messages via the private API.

## Requirements

### Requirement: IMessagingService interface
The app SHALL define an `IMessagingService` interface registered as singleton in DI with methods:
- `CreateConversationAsync(string listingId, CancellationToken ct)` returning `Task<Conversation>`
- `GetConversationsAsync(int limit, int offset, CancellationToken ct)` returning `Task<ConversationsResponse>`
- `GetConversationAsync(int conversationId, CancellationToken ct)` returning `Task<Conversation>`
- `GetMessagesAsync(int conversationId, int limit, int offset, CancellationToken ct)` returning `Task<MessagesResponse>`
- `SendMessageAsync(int conversationId, string body, CancellationToken ct)` returning `Task<Message>`

#### Scenario: Service is injectable
- **WHEN** `IMessagingService` is resolved from DI
- **THEN** a singleton `MessagingService` instance SHALL be returned

### Requirement: Create conversation
`CreateConversationAsync` SHALL POST `{"listing_id":"..."}` to `/api/private/conversations/` with Bearer auth.

#### Scenario: Start conversation about a listing
- **WHEN** `CreateConversationAsync("listing-uuid")` is called
- **THEN** it SHALL POST to `/api/private/conversations/` and return the created `Conversation`

#### Scenario: Rate limited
- **WHEN** the backend returns 429 (trust-level daily limit exceeded)
- **THEN** the exception SHALL propagate to the caller

### Requirement: List conversations
`GetConversationsAsync` SHALL call `GET /api/private/conversations?limit={limit}&offset={offset}` with Bearer auth.

#### Scenario: Fetch conversations page
- **WHEN** `GetConversationsAsync(20, 0)` is called
- **THEN** it SHALL return a `ConversationsResponse` with `Items`, `Limit`, and `Offset`

### Requirement: Get single conversation
`GetConversationAsync` SHALL call `GET /api/private/conversations/{id}` with Bearer auth.

#### Scenario: Fetch conversation detail
- **WHEN** `GetConversationAsync(1)` is called
- **THEN** it SHALL return the `Conversation` with listing info and participant IDs

### Requirement: Get messages
`GetMessagesAsync` SHALL call `GET /api/private/conversations/{id}/messages?limit={limit}&offset={offset}` with Bearer auth.

#### Scenario: Fetch messages page
- **WHEN** `GetMessagesAsync(1, 50, 0)` is called
- **THEN** it SHALL return a `MessagesResponse` with `Items`, `Limit`, and `Offset`

### Requirement: Send message
`SendMessageAsync` SHALL POST `{"body":"..."}` to `/api/private/conversations/{id}/messages` with Bearer auth.

#### Scenario: Send message in conversation
- **WHEN** `SendMessageAsync(1, "Hello!")` is called
- **THEN** it SHALL POST the message and return the created `Message` with id, sender_id, body, and created_at

### Requirement: Conversation model
The app SHALL define a `Conversation` record with: `Id` (int), `ListingId` (string), `ListingTitle` (string?), `BuyerId` (int), `SellerId` (int), `CreatedAt` (string), `UpdatedAt` (string). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize conversation
- **WHEN** the API returns a conversation JSON
- **THEN** it SHALL deserialize to `Conversation` with all fields mapped

### Requirement: ConversationsResponse model
The app SHALL define a `ConversationsResponse` record with: `Items` (List&lt;Conversation&gt;), `Limit` (int), `Offset` (int). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize conversations list
- **WHEN** the API returns a conversations list JSON
- **THEN** it SHALL deserialize to `ConversationsResponse`

### Requirement: Message model
The app SHALL define a `Message` record with: `Id` (int), `ConversationId` (int?), `SenderId` (int), `Body` (string), `CreatedAt` (string). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize message
- **WHEN** the API returns a message JSON
- **THEN** it SHALL deserialize to `Message` with all fields mapped

### Requirement: MessagesResponse model
The app SHALL define a `MessagesResponse` record with: `Items` (List&lt;Message&gt;), `Limit` (int), `Offset` (int). Registered in `SportowyHubJsonContext`.

#### Scenario: Deserialize messages list
- **WHEN** the API returns a messages list JSON
- **THEN** it SHALL deserialize to `MessagesResponse`

### Requirement: CreateConversationRequest model
The app SHALL define a `CreateConversationRequest` record with `ListingId` (string). Registered in `SportowyHubJsonContext`.

#### Scenario: Serialize create conversation request
- **WHEN** `CreateConversationRequest` with ListingId="uuid" is serialized
- **THEN** the JSON SHALL be `{"listing_id":"uuid"}`

### Requirement: SendMessageRequest model
The app SHALL define a `SendMessageRequest` record with `Body` (string). Registered in `SportowyHubJsonContext`.

#### Scenario: Serialize send message request
- **WHEN** `SendMessageRequest` with Body="Hello!" is serialized
- **THEN** the JSON SHALL be `{"body":"Hello!"}`
