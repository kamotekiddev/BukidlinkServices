using System.Reflection;
using Auth.Domain;
using Auth.Infrastructure;
using Auth.Infrastructure.Auth;
using Auth.Infrastructure.Seeders;
using BuildingBlocks.Auth;
using BuildingBlocks.Extensions;
using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("AuthDB"));
});

builder.AddSerilogLogging();

builder.Services.AddCarter();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddValidatorsFromAssemblies([typeof(Program).Assembly]);
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.PropertyNamingPolicy = null; });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DatabaseSeeder.SeedAsync(db);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();
app.UseGlobalExceptionHandler();
app.MapCarter();
app.UseHttpsRedirection();
app.Run();