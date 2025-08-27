using Microsoft.EntityFrameworkCore;
using PatientApi.Application.Interfaces;
using PatientApi.Application.Services;
using PatientApi.Domain.Interfaces;
using PatientApi.Infrastructure.Data;
using PatientApi.Infrastructure.Repositories;
using MassTransit;
using PatientApi.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Entity Framework
builder.Services.AddDbContext<PatientDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<TherapistCreatedConsumer>();
    x.AddConsumer<TherapistUpdatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQ");
        cfg.Host(rabbitMqConfig["Host"], h =>
        {
            h.Username(rabbitMqConfig["Username"]!);
            h.Password(rabbitMqConfig["Password"]!);
        });

        cfg.ReceiveEndpoint("patient-therapist-events", e =>
        {
            e.ConfigureConsumer<TherapistCreatedConsumer>(context);
            e.ConfigureConsumer<TherapistUpdatedConsumer>(context);
        });

        cfg.ConfigureEndpoints(context);
    });
});

// Add repositories
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

// Add services
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
