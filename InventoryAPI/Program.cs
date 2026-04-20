using Carter;
using InventoryAPI.Infrastructure;
using InventoryAPI.Infrastructure.Messaging;
using InventoryAPI.Infrastructure.Messaging.Consumers;
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Infrastructure.Messaging;
using Scalar.AspNetCore;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCarter();
builder.Services.AddSingleton<RabbitMqConnectionFactory>();
builder.Services.AddSingleton<RabbitMqConsumer>();
builder.Services.AddHostedService<ProductVariantCreatedConsumer>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
);

builder.Services.AddCarter();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Inventory API";
        options.Theme = ScalarTheme.Mars;
    });
}

app.MapCarter();
app.UseHttpsRedirection();
app.Run();

