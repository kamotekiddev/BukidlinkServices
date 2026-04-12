using System.Reflection;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Common.Errors;
using ProductCatalogAPI.Infrastructure;
using ProductCatalogAPI.Infrastructure.Messaging;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddValidatorsFromAssemblies([typeof(Program).Assembly]);
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
);

builder.Services.AddCarter();
builder.Services.AddSingleton<RabbitMqConnectionFactory>();
builder.Services.AddSingleton<RabbitMqPublisher>();


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