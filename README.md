# Therapist Platform - Clean Architecture APIs

A comprehensive therapist-patient management platform built with .NET 9 using Clean Architecture principles. The solution consists of two separate APIs: **Therapist API** and **Patient API**, both following Domain-Driven Design (DDD) patterns.

## 🏗️ Architecture Overview

Both APIs are structured using Clean Architecture with four distinct layers:

- **Domain Layer**: Core business entities, enums, and interfaces
- **Application Layer**: Use cases, DTOs, and application services
- **Infrastructure Layer**: Data access, repositories, and external concerns
- **Presentation Layer**: Web API controllers and configuration

## 🚀 Project Structure

```
src/
├── TherapistApi/
│   ├── TherapistApi.Domain/           # Core business logic
│   ├── TherapistApi.Application/      # Use cases and DTOs
│   ├── TherapistApi.Infrastructure/   # Data access layer
│   └── TherapistApi.Presentation/     # Web API controllers
├── PatientApi/
│   ├── PatientApi.Domain/            # Core business logic
│   ├── PatientApi.Application/       # Use cases and DTOs
│   ├── PatientApi.Infrastructure/    # Data access layer
│   └── PatientApi.Presentation/      # Web API controllers
└── Shared/
    └── Shared.Common/                # Shared utilities and base classes
```

## 📋 Features

### Therapist API
- ✅ Complete CRUD operations for therapists
- ✅ Therapist specialization filtering
- ✅ Active therapist queries
- ✅ License validation
- ✅ Availability management

### Patient API
- ✅ Complete CRUD operations for patients
- ✅ Patient status management
- ✅ Medical history tracking
- ✅ Insurance information management
- ✅ Appointment management
- ✅ Emergency contact information

### Shared Features
- ✅ Entity Framework Core integration
- ✅ SQL Server database support
- ✅ Swagger/OpenAPI documentation
- ✅ CORS support
- ✅ Standardized API responses
- ✅ Soft delete implementation
- ✅ Audit fields (CreatedAt, UpdatedAt)

## 🛠️ Technologies Used

- **.NET 9.0** - Latest .NET framework
- **ASP.NET Core Web API** - REST API framework
- **Entity Framework Core 9** - Object-relational mapping
- **SQL Server** - Database (LocalDB for development)
- **Swashbuckle.AspNetCore** - OpenAPI/Swagger documentation
- **Clean Architecture** - Architectural pattern
- **Domain-Driven Design** - Design approach

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK
- SQL Server or SQL Server LocalDB
- Visual Studio 2022 or VS Code

### Installation & Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd therapist
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Run the APIs**

   **Therapist API:**
   ```bash
   cd src/TherapistApi/TherapistApi.Presentation
   dotnet run
   ```
   
   **Patient API:**
   ```bash
   cd src/PatientApi/PatientApi.Presentation
   dotnet run
   ```

5. **Access Swagger Documentation**
   - Therapist API: `https://localhost:7xxx/swagger`
   - Patient API: `https://localhost:7xxx/swagger`

## 📚 API Endpoints

### Therapist API (`/api/therapists`)
- `GET /api/therapists` - Get all therapists
- `GET /api/therapists/{id}` - Get therapist by ID
- `POST /api/therapists` - Create new therapist
- `PUT /api/therapists/{id}` - Update therapist
- `DELETE /api/therapists/{id}` - Delete therapist
- `GET /api/therapists/specialization/{specialization}` - Filter by specialization
- `GET /api/therapists/active` - Get active therapists only

### Patient API (`/api/patients`)
- `GET /api/patients` - Get all patients
- `GET /api/patients/{id}` - Get patient by ID
- `POST /api/patients` - Create new patient
- `PUT /api/patients/{id}` - Update patient
- `DELETE /api/patients/{id}` - Delete patient
- `GET /api/patients/active` - Get active patients only
- `GET /api/patients/{id}/appointments` - Get patient with appointments

## 🗃️ Database Schema

### Therapist Database (TherapistApiDb)
- **Therapists**: Core therapist information, specializations, contact details
- **Availabilities**: Therapist availability schedules

### Patient Database (PatientApiDb)
- **Patients**: Patient demographics, medical history, insurance info
- **Appointments**: Appointment scheduling and session management

## 🏛️ Clean Architecture Benefits

1. **Independence**: Business logic is independent of frameworks, UI, and databases
2. **Testability**: Easy to test business logic in isolation
3. **Maintainability**: Clear separation of concerns makes code easier to maintain
4. **Flexibility**: Easy to swap implementations (e.g., change databases)
5. **Scalability**: Well-organized code structure supports growth

## 🔧 Configuration

Both APIs use `appsettings.json` for configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=[DatabaseName];Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

## 🧪 Development

### Adding New Features
1. Start with the **Domain** layer (entities, enums, interfaces)
2. Implement **Application** services and DTOs
3. Add **Infrastructure** implementations (repositories)
4. Expose through **Presentation** controllers

### Database Migrations
The databases are automatically created on first run. For production environments, consider using EF Core migrations.

## 🚀 Deployment

Both APIs can be deployed independently:
- As separate microservices
- Using containerization (Docker)
- To cloud platforms (Azure, AWS, etc.)

## 📖 Next Steps

- Implement authentication and authorization
- Add comprehensive logging
- Implement health checks
- Add integration tests
- Consider implementing CQRS pattern
- Add API versioning
- Implement real-time notifications

---

**Built with Clean Architecture principles for maintainable, testable, and scalable healthcare management solutions.**
