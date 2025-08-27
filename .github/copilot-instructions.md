<!-- Use this file to provide workspace-specific custom instructions to Copilot. For more details, visit https://code.visualstudio.com/docs/copilot/copilot-customization#_use-a-githubcopilotinstructionsmd-file -->

## Project Overview
This workspace contains two Clean Architecture .NET 9 Web APIs for a therapist-patient management platform:

### APIs Implemented:
1. **Therapist API** (Port: 5136) - Management of therapist profiles, specializations, and availability
2. **Patient API** - Management of patient records, appointments, and medical history

### Architecture:
- **Domain Layer**: Core business entities, enums, and interfaces
- **Application Layer**: Use cases, DTOs, and application services  
- **Infrastructure Layer**: Data access with Entity Framework Core and SQL Server
- **Presentation Layer**: Web API controllers with Swagger documentation

### Key Features:
- Clean Architecture principles with dependency inversion
- Entity Framework Core with SQL Server LocalDB
- Swagger/OpenAPI documentation for both APIs
- CORS support for cross-origin requests
- Standardized API responses with error handling
- Soft delete implementation with audit fields

### Database:
- TherapistApiDb: Therapist profiles and availability schedules
- PatientApiDb: Patient records and appointment management

Both APIs are fully functional with complete CRUD operations and can be run independently as microservices.
