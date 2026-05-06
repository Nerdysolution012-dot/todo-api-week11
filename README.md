# Enhanced TODO API — Week 11

A production-quality RESTful TODO API built with **ASP.NET Core 8 Web API** using an **in-memory** data store. It features DTOs, comprehensive validation, standardized error responses, and proper HTTP status codes.

---

## Project Structure

```
todo-api-week11/
├── Controllers/
│   └── TodosController.cs
├── Models/
│   └── TodoItem.cs
├── DTOs/
│   ├── CreateTodoDto.cs
│   ├── UpdateTodoDto.cs
│   └── TodoResponseDto.cs
├── Responses/
│   └── ErrorResponse.cs
├── Program.cs
├── appsettings.json
├── README.md
└── TodoAPI.postman_collection.json
```

---

## Setup & Run

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Run locally

```bash
# Clone the repository
git clone https://github.com/Nerdysolution012-dot/todo-api-week11.git
cd todo-api-week11/todo-api-week11

# Restore & run
dotnet run
```

The API will start on **http://localhost:5000** (HTTP) and **https://localhost:5001** (HTTPS) by default.  
Swagger UI is available at `http://localhost:5000/swagger` in Development mode.

---

## API Endpoints

### Base URL
```
http://localhost:5000
```

---

### 1. GET /api/todos — Get all TODOs

Returns all TODO items as an array.

**Request:**
```
GET /api/todos
```

**Response: 200 OK**
```json
[
  {
    "id": 1,
    "title": "Complete assignment",
    "description": "Finish Week 11 TODO API",
    "isCompleted": false,
    "createdAt": "2024-03-15T09:30:00Z",
    "dueDate": "2024-03-20T10:00:00Z",
    "priority": "High"
  }
]
```

| Status | Meaning |
|--------|---------|
| 200 OK | Success (empty array if no TODOs) |

---

### 2. GET /api/todos/{id} — Get single TODO

**Request:**
```
GET /api/todos/1
```

**Response: 200 OK**
```json
{
  "id": 1,
  "title": "Complete assignment",
  "description": "Finish Week 11 TODO API",
  "isCompleted": false,
  "createdAt": "2024-03-15T09:30:00Z",
  "dueDate": "2024-03-20T10:00:00Z",
  "priority": "High"
}
```

**Response: 404 Not Found**
```json
{
  "statusCode": 404,
  "message": "Todo with id 99 was not found.",
  "errors": null,
  "timestamp": "2024-03-15T09:30:00Z"
}
```

| Status | Meaning |
|--------|---------|
| 200 OK | TODO found |
| 404 Not Found | TODO does not exist |

---

### 3. POST /api/todos — Create TODO

**Request Body:**
```json
{
  "title": "Complete assignment",
  "description": "Finish Week 11 TODO API",
  "dueDate": "2024-03-20T10:00:00Z",
  "priority": "High"
}
```

**Fields:**
| Field | Type | Required | Constraints |
|-------|------|----------|-------------|
| title | string | ✅ | 3–100 characters, not whitespace-only |
| description | string | ❌ | max 500 characters |
| dueDate | ISO 8601 datetime | ❌ | must not be in the past |
| priority | string | ❌ | `Low`, `Medium`, or `High` (default: `Medium`) |

**Response: 201 Created** (includes `Location` header)
```json
{
  "id": 1,
  "title": "Complete assignment",
  "description": "Finish Week 11 TODO API",
  "isCompleted": false,
  "createdAt": "2024-03-15T09:30:00Z",
  "dueDate": "2024-03-20T10:00:00Z",
  "priority": "High"
}
```

**Response: 400 Bad Request** (validation failure)
```json
{
  "statusCode": 400,
  "message": "Validation failed",
  "errors": {
    "title": ["Title is required"]
  },
  "timestamp": "2024-03-15T09:30:00Z"
}
```

| Status | Meaning |
|--------|---------|
| 201 Created | TODO created successfully |
| 400 Bad Request | Validation failed |

---

### 4. PUT /api/todos/{id} — Update TODO

**Request Body:**
```json
{
  "title": "Updated assignment",
  "description": "Updated description",
  "isCompleted": true,
  "dueDate": "2024-03-25T10:00:00Z",
  "priority": "Medium"
}
```

**Fields:** Same as `CreateTodoDto` plus:
| Field | Type | Required | Constraints |
|-------|------|----------|-------------|
| isCompleted | boolean | ✅ | true or false |

**Response: 200 OK**
```json
{
  "id": 1,
  "title": "Updated assignment",
  "description": "Updated description",
  "isCompleted": true,
  "createdAt": "2024-03-15T09:30:00Z",
  "dueDate": "2024-03-25T10:00:00Z",
  "priority": "Medium"
}
```

| Status | Meaning |
|--------|---------|
| 200 OK | Updated successfully |
| 400 Bad Request | Validation failed |
| 404 Not Found | TODO does not exist |

---

### 5. DELETE /api/todos/{id} — Delete TODO

**Request:**
```
DELETE /api/todos/1
```

**Response: 204 No Content** — No body returned.

| Status | Meaning |
|--------|---------|
| 204 No Content | Deleted successfully |
| 404 Not Found | TODO does not exist |

---

## Validation Rules

| Rule | Description |
|------|-------------|
| Title required | Title field must be present |
| Title length | 3–100 characters |
| Title not whitespace | Title cannot consist of only spaces |
| Description length | Max 500 characters |
| Priority values | Must be `Low`, `Medium`, or `High` |
| DueDate not past | DueDate (if provided) must not be in the past |

---

## Standard Error Response

All errors return an `ErrorResponse` JSON object:

```json
{
  "statusCode": 400,
  "message": "Validation failed",
  "errors": {
    "title": ["Title is required"],
    "dueDate": ["DueDate cannot be in the past."]
  },
  "timestamp": "2024-03-15T09:30:00Z"
}
```

| Field | Type | Description |
|-------|------|-------------|
| statusCode | int | HTTP status code (400, 404, 500) |
| message | string | Human-readable summary |
| errors | object\|null | Field-level validation errors (null for 404/500) |
| timestamp | ISO 8601 | UTC time the error occurred |

---

## Postman Collection

Import `TodoAPI.postman_collection.json` (in the repo root) into Postman.  
Set the `baseUrl` variable to `http://localhost:5000` before running.

The collection includes pre-configured requests for all endpoints including valid/invalid scenarios.
