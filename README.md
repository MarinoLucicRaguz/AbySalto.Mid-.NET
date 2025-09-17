# AbySalto – Technical Assignment - Mid

##  Task Description
The goal was to implement a system that manages **users, baskets, and products** within the given project structure.  
The application uses **DummyJSON API** as an external service for fetching product data and allows users to add products to their basket.  

Base skeleton was provided:  
- [.NET](https://github.com/Abysalto/mid.net)  

## Application Features

### Users
- User registration  
- User login  
- Fetch currently logged-in user  

### Products
- Fetch all products (with pagination & sorting)  
- Fetch single product  
- Add product to favorites  

### Basket
- Add product to basket  
- Remove product from basket  
- Fetch current user’s basket  

### Bonus Features
- Project structured following **Clean Architecture** principles  
- **Data caching** to reduce DummyJSON API calls  
- **Pagination and sorting** for products  

### Technical Requirements
- SQL Server database for persisting data  
- Backend in .NET  
- Frontend in React  

---

## Project Structure
- AbySalto.Mid.Domain/ # Domain entities
- AbySalto.Mid.Application/ # DTOs, services, application logic
- AbySalto.Mid.Infrastructure/ # Repositories, DummyJSON integration, caching
- AbySalto.Mid.WebApi/ # ASP.NET Core Web API (controllers)
- AbySalto.Frontend/ # React frontend application
- AbySalto.Mid.Tests/ # Unit and integration tests (xUnit)


---

## Setup & Run

### 1. Backend (.NET API)

Configure **`appsettings.Development.json`**:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=AbySalto;User Id=sa;Password=YourPassword;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "your-secret-key",
    "Issuer": "AbySalto.Mid",
    "Audience": "AbySalto.Mid.Client",
    "ExpireMinutes": 15,
    "RefreshTokenDays": 7
  },
  "ExternalApis": {
    "DummyJson": {
      "BaseUrl": "https://dummyjson.com/",
      "TimeoutSeconds": 10,
      "CacheSeconds": 60
    }
  },
  "AllowedHosts": "*"
} 
```

Replace YOUR_SERVER and YourPassword with your local SQL Server settings.

Apply EF Core migrations:
```
cd AbySalto.Mid
dotnet ef database update
```

Run the API:
```
dotnet run --project AbySalto.Mid.WebApi
```

Backend available at:
  - **API: https://localhost:5001**
  - **Swagger UI: https://localhost:5001/swagger**


### 2. Frontend (React)

Navigate into frontend folder:
```
cd AbySalto.Frontend
```
Create a **.env** file
```
VITE_API_URL=https://localhost:5001/api
```
Install dependencies:
```
npm install
```

Start the dev server:
```
npm run dev
```

Frontend available at:
**http://localhost:5173**

## Usage

- Start backend and frontend.
- Open frontend in browser at http://localhost:5173.
- Register a new user (Register page).
- After registering you can log in, browse products, add to basket, and mark favorites.

## Tests

- Unit tests: ProductMapperTests, ProductServiceTests, ServiceResponseTests
- To run backend tests:
```
dotnet test
```

## Implemented

- User registration, login, and fetch current user
- Basket: add, remove, and fetch items
- Products: list all, get by id, add to favorites
- Pagination & sorting
- In-memory caching
- Clean Architecture structure
- Unit + integration tests
- React frontend for API interaction

## Features
- Auth
  - Access token (short-lived) is returned in JSON and stored in Redux (state.auth.token).
  - Refresh token (long-lived) is stored only in an HTTP-only cookie (never accessible to JS).
  - Axios interceptor behavior:
  - Add Authorization: Bearer <access-token> to requests.
  - If a response is 401, call POST /api/user/refresh (cookie sent automatically).
  - On success, update Redux with the new access token and retry the original request.
  - If refresh fails (expired/invalid), clear auth & redirect to /login.

## Frontend State & Data-Fetching

- Redux Toolkit
  - Slice: auth (token, user).
  - Persistence: localStorage
  - Clear on logout or failed refresh.

- React Query
  - useQuery for products, favorites, basket (caching + loading/error UI).
  - useMutation for add/remove basket & favorites, login, register, logout.

- Axios client
 - withCredentials: true (refresh cookie travels with requests)
 - Request interceptor attaches Authorization.

## API Endpoints Overview

- All endpoints return a ServiceResponse<T> wrapper, created via the shared HandleResponse method in BaseController.
  - On success → response contains data, success = true, statusCode (usually 200/201).
  - On error → response contains success = false, message, statusCode.

- Exceptions are caught and shaped consistently via the GlobalExceptionHandler.

- UserController (/api/user)
  - POST /register - Register a new user. Sets an HTTP-only refresh token cookie and returns an access token + user.
  - POST /login - Authenticate a user. Sets a refresh token cookie and returns an access token + user.
  - POST /refresh - Exchange a refresh token (from cookie) for a new access token. Refresh cookie is rotated.
  - POST /logout - Revokes the current refresh token (from cookie) and deletes it.
  - GET / (authorized) - Get the current logged-in user’s data.

- ProductController (/api/product)
  - GET /getallpaginated?page=1&size=10&sortBy=title&order=asc – Fetch a paginated list of products (basic details). Supports pagination & sorting.
  - GET /{id} – Fetch a single product by id (lightweight ProductDto).
  - GET /getdetailsbyid/{id} – Fetch full product details (ProductDetailExtendedDto), including extra fields (images, tags, reviews, meta).

- FavoriteController (/api/favorite) (authorized)
  - POST /{productId} – Add a product to the current user’s favorites.
  - DELETE /{productId} – Remove a product from the current user’s favorites.
  - GET / – Get all favorites for the current user.

- BasketController (/api/basket) (authorized)
  - GET / – Get the current user’s basket.
  - POST /add/{productId}?increment=1 – Add product to basket (or increase its quantity).
  - POST /reduce/{productId}?decrement=1 – Reduce quantity of a basket item.
  - DELETE /{productId} – Remove a product completely from basket.
  - DELETE / – Clear the entire basket for the current user.

## Common Infrastructure

- HandleResponse (BaseController)
  - Centralizes API response shaping.
  - HandleResponse(response) → returns ServiceResponse<T> with status code.
  - HandleResponse(response, transform) → transforms DTOs (e.g., from AuthResult → AuthResponseDto) while keeping the same response envelope.

- GlobalExceptionHandler
  - Registered via services.AddExceptionHandler<GlobalExceptionHandler>().
