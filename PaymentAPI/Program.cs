using System.Reflection;
using BuildingBlocks.Extensions;
using Carter;
using Microsoft.EntityFrameworkCore;
using PaymentAPI.Extensions;
using PaymentAPI.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PaymentDb"));
});

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
);

builder.Services.AddCarter();
builder.AddSerilogLogging();
builder.Services.AddXenditHttpClient(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseGlobalExceptionHandler();
app.UseHttpsRedirection();
app.UseSerilogRequestLogging();

app.MapCarter();
app.Run();