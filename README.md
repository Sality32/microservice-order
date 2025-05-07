# microservice-order
This is for mini microservices projects

## Structure Solution
- UserApp (service for User management)
- ProductApp (service for Product Management)
- OrderApp (service for Management Order)

## Structure Service
- Controllers (Presentation)
- Core
  - Application (Logic business)
    - DTOs
    - Interface (Service Contracts)
    - Service 
  - Domain (Business Entities and rules)
    - Entities (Core Entity)
    - Interfaces (Repository Contracts)
- Infrastructure
  - Data
    - Context (Entities Configuration and Database Context)
  - Repositories (Data implementation)
- Migrations (File generator for database migration)
- Properties (Configuration project)

## Tools 
- PostgreSQL 11
- .NET 8

## Preparation
1. Install .NET 8 and PostgreSQL
2. Preparation Database (create databases):
   - micro_user
   - micro_product
   - micro_order

## Steps to Run
1. Clone this project
2. Navigate to solution directory:
   ```bash
   cd microservice-order

   dotnet restore

   ```
3. Create appsettings.json each service and add your configuration
  - User App and Product App need credential for database
  - Order App need credential database and userService url and productService url
  - Configuration every url can find at properties/launchsetting.json at every app
4. Run each service with 
  ```bash
  cd UserApp
  dotnet run

  cd ProductApp
  dotnet run

  cd OrderApp
  dotnet run
  ```

5. Access swagger with {urlService}/swagger