using System.Reflection;
using System.Text.Json.Serialization;
using BuildingBlocks.Extensions;
using Carter;
using InventoryAPI.Features.Inventories.ReleaseStock;
using InventoryAPI.Infrastructure;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("InventoryDb"));
});

builder.Services.AddMassTransit(busConfigurator =>
{
    var rabbitMqConfig = builder.Configuration.GetSection("RabbitMq");

    busConfigurator.AddConsumer<ReleaseStocksConsumer>();

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
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseGlobalExceptionHandler();
app.MapCarter();
app.UseHttpsRedirection();
app.Run();