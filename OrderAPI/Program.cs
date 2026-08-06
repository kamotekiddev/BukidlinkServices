using System.Reflection;
using System.Text.Json.Serialization;
using BuildingBlocks.Auth;
using BuildingBlocks.Extensions;
using Carter;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Infrastructure;
using OrderAPI.Infrastructure.HttpClients.ProductServiceClient;
using OrderAPI.Infrastructure.HttpClients.StoreServiceClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
);

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqConfig = builder.Configuration.GetSection("RabbitMq");
        cfg.ConfigureEndpoints(context);
        cfg.Host(rabbitMqConfig["Host"], rabbitMqConfig["VirtualHost"], hostConfigurator =>
        {
            hostConfigurator.Username(rabbitMqConfig["Username"]!);
            hostConfigurator.Password(rabbitMqConfig["Password"]!);
        });
    });
});

builder.Services.AddCarter();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddHttpClient<IProductServiceClient, ProductServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:Product:Url"]!);
});

builder.Services.AddHttpClient<IStoreServiceClient, StoreServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:Store:Url"]!);
});

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
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();

app.Run();