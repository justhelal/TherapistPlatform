# MassTransit Integration for Therapist Platform

This document describes the MassTransit and RabbitMQ integration implemented for communication between the Therapist API and Patient API.

## Overview

The system uses MassTransit with RabbitMQ for event-driven communication between the two APIs:
- **TherapistApi**: Publishes therapist-related events, consumes patient and appointment events
- **PatientApi**: Publishes patient and appointment events, consumes therapist events

## Prerequisites

### 1. RabbitMQ Installation
You need to have RabbitMQ running locally. 

#### Option 1: Docker (Recommended)
```bash
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

#### Option 2: Local Installation
- Download and install RabbitMQ from: https://www.rabbitmq.com/download.html
- Enable the management plugin: `rabbitmq-plugins enable rabbitmq_management`
- Access management UI at: http://localhost:15672 (guest/guest)

## Configuration

### Connection Settings
Both APIs are configured with the following RabbitMQ settings in `appsettings.json`:

```json
{
  "RabbitMQ": {
    "Host": "localhost",
    "Port": 5672,
    "Username": "guest",
    "Password": "guest",
    "VirtualHost": "/"
  }
}
```

## Event Contracts

Located in `Shared.Events` library:

### 1. TherapistCreatedEvent
Published when a new therapist is created.
```csharp
public record TherapistCreatedEvent
{
    public Guid TherapistId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Specialization { get; init; }
    public DateTime CreatedAt { get; init; }
}
```

### 2. TherapistUpdatedEvent
Published when therapist information is updated.

### 3. PatientCreatedEvent
Published when a new patient is registered.
```csharp
public record PatientCreatedEvent
{
    public Guid PatientId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public DateTime DateOfBirth { get; init; }
    public DateTime CreatedAt { get; init; }
}
```

### 4. AppointmentScheduledEvent
Published when a new appointment is scheduled.
```csharp
public record AppointmentScheduledEvent
{
    public Guid AppointmentId { get; init; }
    public Guid PatientId { get; init; }
    public Guid TherapistId { get; init; }
    public DateTime AppointmentDateTime { get; init; }
    public string Notes { get; init; }
    public DateTime CreatedAt { get; init; }
}
```

### 5. AppointmentCancelledEvent
Published when an appointment is cancelled.

## Message Flow

### TherapistApi
**Publishes:**
- `TherapistCreatedEvent` when POST /api/therapists is called
- `TherapistUpdatedEvent` when PUT /api/therapists/{id} is called

**Consumes:**
- `PatientCreatedEvent` → Logs patient creation for reference
- `AppointmentScheduledEvent` → Updates therapist availability/schedule

**Queue Names:**
- `therapist-patient-events` (receives patient events)
- `therapist-appointment-events` (receives appointment events)

### PatientApi
**Publishes:**
- `PatientCreatedEvent` when POST /api/patients is called
- `AppointmentScheduledEvent` when POST /api/appointments is called
- `AppointmentCancelledEvent` when DELETE /api/appointments/{id} is called

**Consumes:**
- `TherapistCreatedEvent` → Updates available therapists cache
- `TherapistUpdatedEvent` → Updates therapist information

**Queue Names:**
- `patient-therapist-events` (receives therapist events)

## API Endpoints

### Appointment Management (PatientApi)
- `POST /api/appointments` - Schedule new appointment (publishes AppointmentScheduledEvent)
- `PUT /api/appointments/{id}` - Update appointment
- `DELETE /api/appointments/{id}` - Cancel appointment (publishes AppointmentCancelledEvent)
- `GET /api/appointments/patient/{patientId}` - Get patient appointments
- `GET /api/appointments/upcoming` - Get upcoming appointments

## Testing the Integration

### 1. Start RabbitMQ
```bash
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

### 2. Run Both APIs
```bash
# Terminal 1: TherapistApi
cd src/TherapistApi/TherapistApi.Presentation
dotnet run

# Terminal 2: PatientApi  
cd src/PatientApi/PatientApi.Presentation
dotnet run
```

### 3. Test Event Publishing

#### Create a Therapist (publishes TherapistCreatedEvent)
```bash
curl -X POST http://localhost:5136/api/therapists \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@example.com",
    "phoneNumber": "555-0123",
    "licenseNumber": "LIC001",
    "specialization": "CognitiveBehavioral",
    "licenseExpiryDate": "2025-12-31",
    "yearsOfExperience": 5,
    "biography": "Experienced therapist",
    "hourlyRate": 150.00,
    "address": "123 Main St",
    "city": "Anytown",
    "state": "ST",
    "zipCode": "12345",
    "country": "USA"
  }'
```

#### Create a Patient (publishes PatientCreatedEvent)
```bash
curl -X POST http://localhost:5137/api/patients \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Jane",
    "lastName": "Smith", 
    "email": "jane.smith@example.com",
    "phoneNumber": "555-0124",
    "dateOfBirth": "1990-01-15",
    "gender": "Female",
    "address": "456 Oak St",
    "city": "Anytown",
    "state": "ST",
    "zipCode": "12345",
    "country": "USA"
  }'
```

#### Schedule an Appointment (publishes AppointmentScheduledEvent)
```bash
curl -X POST http://localhost:5137/api/appointments \
  -H "Content-Type: application/json" \
  -d '{
    "patientId": "PATIENT_GUID_HERE",
    "therapistId": "THERAPIST_GUID_HERE",
    "appointmentDate": "2025-09-01T10:00:00Z",
    "duration": "00:50:00",
    "notes": "Initial consultation",
    "cost": 150.00
  }'
```

### 4. Monitor Events
- Check application logs to see event consumption
- Use RabbitMQ Management UI (http://localhost:15672) to monitor queues
- Verify message flow between services

## Benefits

1. **Loose Coupling**: Services communicate through events, not direct API calls
2. **Scalability**: Easy to add new services that consume existing events
3. **Reliability**: RabbitMQ ensures message delivery with persistence
4. **Observability**: Built-in monitoring and logging of message flow
5. **Extensibility**: Simple to add new event types and consumers

## Future Enhancements

1. **Saga Orchestration**: Handle complex workflows spanning multiple services
2. **Dead Letter Queues**: Handle failed message processing
3. **Message Encryption**: Secure sensitive patient/therapist data in transit
4. **Event Sourcing**: Store all events for audit and replay capabilities
5. **Health Checks**: Monitor RabbitMQ connectivity and queue health
