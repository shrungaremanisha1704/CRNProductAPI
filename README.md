# CRN Product API (.NET 8)

RESTful Products and Items API built with ASP.NET Core Web API, EF Core, SQL Server, JWT access tokens and rotating refresh tokens.

## Features
- Product CRUD: `GET/POST /api/v1/Products`, `GET/PUT/DELETE /api/v1/Products/{id}`
- Item CRUD with Product foreign-key validation
- JWT register/login/refresh endpoints
- Password hashing using ASP.NET Core `PasswordHasher`
- Refresh token hashes stored in database and one-time rotation
- Repository + service layers and dependency injection
- EF Core async queries, `AsNoTracking`, indexes and pagination
- FluentValidation ASP.NET Core integration, DataAnnotations request validation
- Central exception middleware with trace ID and structured Serilog console logging
- Swagger/OpenAPI Bearer authorization
- HTTPS redirect, CORS policy, response compression, security headers
- xUnit/Moq unit tests
- Dockerfile and Docker Compose

## Run locally (Windows / Visual Studio)
Requirements: .NET 8 SDK, Visual Studio 2022, SQL Server LocalDB.

1. Extract the ZIP.
2. Open `CRNProductAPI.sln` in Visual Studio.
3. Check `src/CRNProductAPI/appsettings.json`. The default connection string is set to `(localdb)\\ProjectModels`. If your LocalDB instance is `(localdb)\\MSSQLLocalDB`, change the connection string to that instance.
4. The app has a clearly marked local-development JWT key in appsettings. Do not use it for production. For a public repository, prefer moving the key to User Secrets before publishing:
   - Right-click the API project in Visual Studio > Manage User Secrets.
   - Add JSON:
     {
       "Jwt": {
         "Key": "put-a-long-random-local-secret-here"
       }
     }
   - Remove `Jwt:Key` from appsettings.json before committing to GitHub.
5. Set `CRNProductAPI` as Startup Project and press F5 (or Ctrl+F5).
6. Swagger opens at the local URL shown in Visual Studio, usually `https://localhost:7xxx/swagger`.
7. Register a test account using `POST /api/v1/Auth/register`, then login using `POST /api/v1/Auth/login`.
8. Copy the `accessToken` from login response. Click Swagger **Authorize**, paste only the token value (do not add `Bearer `), click Authorize, then Close.
9. Test `GET /api/v1/Products`. To create a product, use `POST /api/v1/Products` with:
   {
     "productName": "Keyboard",
     "createdBy": "Manisha"
   }
   Use the returned ID for GET/PUT/DELETE. Products and Items endpoints require authentication.

The application calls `EnsureCreated()` at startup for easy local assessment use. For a production system, replace this with reviewed EF Core migrations.

## Test
From the solution directory:
```powershell
dotnet test
```

## Docker Compose
Install Docker Desktop and start it, then from the solution directory:
```powershell
docker compose up --build
```
The API listens on `http://localhost:8080`. For a containerized database, Compose uses SQL Server 2022 and a local-only example password. Change all development credentials before using outside a local machine. Do not use the Docker example credentials in production.

## API endpoints
- `POST /api/v1/Auth/register`
- `POST /api/v1/Auth/login`
- `POST /api/v1/Auth/refresh`
- `GET /api/v1/Products?pageNumber=1&pageSize=20`
- `GET /api/v1/Products/{id}`
- `POST /api/v1/Products`
- `PUT /api/v1/Products/{id}`
- `DELETE /api/v1/Products/{id}`
- `GET /api/v1/Items?pageNumber=1&pageSize=20`
- `GET /api/v1/Items/{id}`
- `POST /api/v1/Items`
- `PUT /api/v1/Items/{id}`
- `DELETE /api/v1/Items/{id}`

## Assessment submission checklist
- Run the app and capture a screenshot showing Swagger locally (Win+Shift+S).
- Create your own **public** GitHub repository and push the project, after removing development JWT keys and reviewing all files for credentials.
- Email HR with the exact subject: `CRN Technical Assessment Complete Successfully`.
- Include the public GitHub repository link and attach the screenshot.
