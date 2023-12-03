using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserService.Consumers;
using UserService.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Entity Framework Core with In-Memory Database
builder.Services.AddDbContext<UserServiceContext>(options =>
    options.UseInMemoryDatabase("UserServiceDatabase"));

builder.Services.AddMassTransit(x =>
{
    x.AddActivitiesFromNamespaceContaining<DeleteUserConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
