using System.Text;
using BuildingBlocks.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BuildingBlocks.Auth;

public static class AuthenticationExtension
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Secret"]!)),
                ValidateIssuerSigningKey = true,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                ValidateIssuer = true,
                ValidIssuer = config["Jwt:Issuer"],

                ValidateAudience = true,
                ValidAudience = config["Jwt:Audience"]
            };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = async context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsJsonAsync(new
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Messaage = "Unauthenticated."
                    });
                },
                OnForbidden = async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsJsonAsync(new
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Message =
                            "You do not have permission to access this resource."
                    });
                }
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policy.Farmer, configurePolicy => { configurePolicy.RequireRole(Roles.Farmer); });
            options.AddPolicy(Policy.Customer, configurePolicy => { configurePolicy.RequireRole(Roles.Customer); });
            options.AddPolicy(Policy.Admin, configurePolicy => { configurePolicy.RequireRole(Roles.Admin); });
        });


        return services;
    }
}