using Microsoft.EntityFrameworkCore;
using TherapistApi.Application.Interfaces;
using TherapistApi.Application.Services;
using TherapistApi.Domain.Interfaces;
using TherapistApi.Infrastructure.Data;
using TherapistApi.Infrastructure.Repositories;
using MassTransit;
using TherapistApi.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Entity Framework
builder.Services.AddDbContext<TherapistDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PatientCreatedConsumer>();
    x.AddConsumer<AppointmentScheduledConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQ");
        cfg.Host(rabbitMqConfig["Host"], h =>
        {
            h.Username(rabbitMqConfig["Username"]!);
            h.Password(rabbitMqConfig["Password"]!);
        });

        cfg.ReceiveEndpoint("therapist-patient-events", e =>
        {
            e.ConfigureConsumer<PatientCreatedConsumer>(context);
        });

        cfg.ReceiveEndpoint("therapist-appointment-events", e =>
        {
            e.ConfigureConsumer<AppointmentScheduledConsumer>(context);
        });

        cfg.ConfigureEndpoints(context);
    });
});

// Add repositories
builder.Services.AddScoped<ITherapistRepository, TherapistRepository>();

// Add services
builder.Services.AddScoped<ITherapistService, TherapistService>();

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
