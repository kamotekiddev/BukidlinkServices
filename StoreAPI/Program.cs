using System.Reflection;
using BuildingBlocks.Auth;
using BuildingBlocks.Extensions;
using Carter;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using StoreAPI.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("StoreDB");
    options.UseNpgsql(connectionString);
});

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
    app.MapScalarApiReference(options =>
    {
        options.Title = "Store API";
        options.Theme = ScalarTheme.Kepler;
    });
}

app.UseAuthentication();
app.UseAuthorization();
app.UseGlobalExceptionHandler();
app.MapCarter();
app.UseHttpsRedirection();
app.Run();