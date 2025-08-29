
# Therapist Platform

Simple .NET 9 platform for managing therapists and patients. Two microservices: Therapist API and Patient API. Built with Clean Architecture and Entity Framework Core.




## Overview

This solution contains:
- **Patient API**: CRUD for patients and appointments
- **Therapist API**: CRUD for therapists and schedules




## Features

- Therapist and patient management
- Appointment scheduling
- Therapist availability


## Tech Stack

- .NET 9
- Entity Framework Core
- SQL Server
- Swagger/OpenAPI


## Project Structure

src/
    PatientApi/
        PatientApi.Presentation/
        PatientApi.Application/
        PatientApi.Domain/
        PatientApi.Infrastructure/
    TherapistApi/
        TherapistApi.Presentation/
        TherapistApi.Application/
        TherapistApi.Domain/
        TherapistApi.Infrastructure/
    Shared/
        Shared.Common/
        Shared.Events/




## Getting Started

1. Clone the repo
2. Update connection strings in `appsettings.json` for both APIs
3. Run migrations:
    ```
    cd src/TherapistApi/TherapistApi.Infrastructure
    dotnet ef database update
    cd ../../PatientApi/PatientApi.Infrastructure
    dotnet ef database update
    ```
4. Run each API:
    ```
    # Therapist API (runs on port 5136)
    dotnet run --project src/TherapistApi/TherapistApi.Presentation
    
    # Patient API (runs on port 5027) - in a new terminal
    dotnet run --project src/PatientApi/PatientApi.Presentation
    ```

## How to Run

### Prerequisites
- .NET 9 SDK
- SQL Server (configured for server "Helal" or update connection strings)

### API URLs
- **Therapist API**: http://localhost:5136 | https://localhost:7139
- **Patient API**: http://localhost:5027 | https://localhost:7242

### Swagger Documentation
- Therapist API: http://localhost:5136/swagger
- Patient API: http://localhost:5027/swagger


## API Endpoints

Patient API:
- `GET /api/patients`
- `GET /api/patients/{id}`
- `POST /api/patients`
- `PUT /api/patients/{id}`
- `DELETE /api/patients/{id}`
- `GET /api/appointments`
- `POST /api/appointments`
- `PUT /api/appointments/{id}`

Therapist API:
- `GET /api/therapists`
- `GET /api/therapists/{id}`
- `POST /api/therapists`
- `PUT /api/therapists/{id}`
- `GET /api/schedule`
- `POST /api/schedule`










## License

MIT


