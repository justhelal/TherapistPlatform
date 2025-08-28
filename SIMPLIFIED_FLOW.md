# Simplified Appointment Flow

## Overview
This project has been refactored to implement a simple appointment creation flow between Patient API and Therapist API using RabbitMQ messaging.

## Flow Description

### 1. Patient API - Appointment Creation
**Endpoint:** `POST /api/appointments`

**Process:**
1. Validates patient + therapist ID (basic validation)
2. Saves appointment in PatientApi database 
3. Publishes `AppointmentCreatedEvent` to RabbitMQ

**Event Structure:**
```json
{
  "appointmentId": "guid",
  "patientId": "guid", 
  "therapistId": "guid",
  "dateTime": "2025-09-01T10:00:00Z"
}
```

### 2. RabbitMQ Message Flow
- **Exchange:** `appointments.exchange`
- **Queue:** `therapist-appointments-queue` 
- **Consumer:** Therapist API listens to this queue

### 3. Therapist API - Schedule Update
**Consumer:** `AppointmentCreatedConsumer` (located in `TherapistApi.Infrastructure.Messaging`)

**Process:**
1. Receives `AppointmentCreatedEvent` from RabbitMQ
2. Creates `TherapistSchedule` entry in TherapistApi database
3. Marks time slot as blocked for the therapist

## API Endpoints

### Patient API
- `POST /api/appointments` - Create new appointment

### Therapist API  
- `GET /api/schedule/therapist/{therapistId}` - View therapist schedule
- `GET /api/schedule/therapist/{therapistId}/availability?dateTime=2025-09-01T10:00:00Z` - Check availability

## Testing the Flow

1. **Start RabbitMQ:**
   ```bash
   docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
   ```

2. **Start both APIs:**
   ```bash
   dotnet run --project src/PatientApi/PatientApi.Presentation
   dotnet run --project src/TherapistApi/TherapistApi.Presentation  
   ```

3. **Create an appointment:**
   ```bash
   curl -X POST http://localhost:5000/api/appointments \
   -H "Content-Type: application/json" \
   -d '{
     "patientId": "123e4567-e89b-12d3-a456-426614174000",
     "therapistId": "987fcdeb-51a2-43d1-9c45-123456789abc", 
     "appointmentDate": "2025-09-01T10:00:00Z",
     "duration": "01:00:00",
     "notes": "Initial consultation",
     "cost": 150.00
   }'
   ```

4. **Check therapist schedule:**
   ```bash
   curl http://localhost:5136/api/schedule/therapist/987fcdeb-51a2-43d1-9c45-123456789abc
   ```

## Key Components Removed/Simplified

- ❌ Patient management (create/update patients)
- ❌ Therapist management (create/update therapists)  
- ❌ Appointment updates/cancellations
- ❌ Complex cross-service event handling
- ❌ Patient-therapist event consumers in Patient API

## Key Components Kept

- ✅ Appointment creation in Patient API
- ✅ `AppointmentCreatedEvent` publishing 
- ✅ RabbitMQ messaging infrastructure
- ✅ Therapist schedule management
- ✅ Event consumption in Therapist API
- ✅ Basic validation and error handling

This simplified version focuses entirely on the core appointment flow you described while maintaining clean architecture and proper event-driven design.
