using System.Reflection;
using Auth.Domain;
using Auth.Infrastructure;
using Auth.Infrastructure.Auth;
using BuildingBlocks.Extensions;
using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("AuthDB"));
});

builder.Services.AddCarter();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddValidatorsFromAssemblies([typeof(Program).Assembly]);
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
);
builder.Services.AddHttpContextAccessor();
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Auth API";
        options.Theme = ScalarTheme.DeepSpace;
    });
}

app.UseAuthentication();
app.UseAuthorization();
app.UseGlobalExceptionHandler();
app.MapCarter();
app.UseHttpsRedirection();
app.Run();