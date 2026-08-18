using System.Reflection;
using BuildingBlocks.Auth;
using BuildingBlocks.Extensions;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StoreAPI.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddValidatorsFromAssemblies([typeof(Program).Assembly]);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("StoreDB");
    options.UseNpgsql(connectionString);
});

builder.AddSerilogLogging();

builder.Services.AddMediatR(options =>
{
    var assembly = Assembly.GetExecutingAssembly();
    options.RegisterServicesFromAssembly(assembly);
});

builder.Services.AddCarter();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseGlobalExceptionHandler();

app.MapCarter();

app.Run();