using System.Reflection;
using Carter;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Common.Errors;
using ProductCatalogAPI.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddValidatorsFromAssemblies([typeof(Program).Assembly]);
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
);

builder.Services.AddMassTransit(busConfigurator =>
{
    var rabbitMqConfig = builder.Configuration.GetSection("RabbitMq");
    busConfigurator.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(rabbitMqConfig["Host"], "/", host =>
        {
            host.Username(rabbitMqConfig["Username"]!);
            host.Password(rabbitMqConfig["Password"]!);
        });
        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddCarter();

var app = builder.Build();
app.UseGlobalExceptionHandler();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Product Catalog API";
        options.Theme = ScalarTheme.Mars;
    });
}

app.MapCarter();
app.UseHttpsRedirection();
app.Run();