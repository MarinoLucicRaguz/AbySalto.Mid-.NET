# AbySalto – Technical Assignment - Mid

##  Task Description
The goal was to implement a system that manages **users, baskets, and products** within the given project structure.  
The application uses **DummyJSON API** as an external service for fetching product data and allows users to add products to their basket.  

Base skeleton was provided:  
- [.NET](https://github.com/Abysalto/mid.net)  

## ✅ Application Features

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
- ✅ SQL Server database for persisting data  
- ✅ Backend in .NET  
- ✅ Frontend in React  

---

## 📂 Project Structure
- AbySalto.Mid.Domain/ # Domain entities
- AbySalto.Mid.Application/ # DTOs, services, application logic
- AbySalto.Mid.Infrastructure/ # Repositories, DummyJSON integration, caching
- AbySalto.Mid.WebApi/ # ASP.NET Core Web API (controllers)
- AbySalto.Frontend/ # React frontend application
- AbySalto.Mid.Tests/ # Unit and integration tests (xUnit)


---

## ⚙️ Setup & Run

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
