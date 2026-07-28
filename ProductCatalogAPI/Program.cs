using System.Reflection;
using BuildingBlocks.Auth;
using BuildingBlocks.Extensions;
using Carter;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Infrastructure;
using ProductCatalogAPI.Infrastructure.HttpClients.StoreHttpClient;

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

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddCarter();

builder.Services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.PropertyNamingPolicy = null; });

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddHttpClient<StoreClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:Store:Url"]!);
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