# API Documentation

Swagger/OpenAPI is configured in the API project (`ServiceCollectionExtensions.cs`).

- Swagger UI available in Development at `/swagger`.
- OpenAPI JSON generated at `/swagger/v1/swagger.json`.
- Security: Bearer JWT.

## Main resources

- Auth (`/api/auth`)
- Artists (`/api/artists`)
- Artist Performances (`/api/artist-performances`)
- Events (`/api/events`)
- Event Venues (`/api/event-venues`)
- Event Venue Calendars (`/api/event-venue-calendars`)
- Genres (`/api/genres`)
- Venues (`/api/venues`)
- Seating Maps (`/api/seating-maps`)
- Seating Sections (`/api/seating-sections`)
- Seating Rows (`/api/seating-rows`)
- Seating Seats (`/api/seating-seats`)

Each resource supports standard CRUD and filtering/pagination where applicable.
Idempotency header (`Idempotency-Key`) is supported on mutating endpoints.

## Example: Get token

```http
POST /api/auth/token
Content-Type: application/json

{
  "username": "admin",
  "password": "secret"
}
```

## Example: Create venue

```http
POST /api/venues
Authorization: Bearer <token>
Content-Type: application/json
Idempotency-Key: 11111111-2222-3333-4444-555555555555

{
  "name": "Main Hall",
  "address": "123 Street"
}
```
