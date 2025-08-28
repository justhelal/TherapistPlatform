using Microsoft.EntityFrameworkCore;
using TherapistApi.Application.Interfaces;
using TherapistApi.Application.Services;
using TherapistApi.Domain.Interfaces;
using TherapistApi.Infrastructure.Data;
using TherapistApi.Infrastructure.Repositories;
using TherapistApi.Infrastructure.Messaging;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Entity Framework
builder.Services.AddDbContext<TherapistDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add MassTransit with RabbitMQ - simplified configuration
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<Consumers>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Configure specific exchange and queue for appointments
        cfg.ReceiveEndpoint("therapist-appointments-queue", e =>
        {
            e.ConfigureConsumer<Consumers>(context);
            e.Bind("appointments.exchange");
        });
    });
});

// Add repositories
builder.Services.AddScoped<ITherapistRepository, TherapistRepository>();
builder.Services.AddScoped<ITherapistScheduleRepository, TherapistScheduleRepository>();

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
