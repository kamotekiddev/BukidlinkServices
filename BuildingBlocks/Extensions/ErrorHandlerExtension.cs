using System.Text.Json;
using BuildingBlocks.Errors;
using BuildingBlocks.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Extensions;

public class GlobalExceptionHandler
{
};

public static class ExceptionHandlingExtensions
{
    public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(builder =>
        {
            builder.Run(async context =>
            {
                var logger = context.RequestServices
                    .GetRequiredService<ILogger<GlobalExceptionHandler>>();

                var feature = context.Features
                    .Get<IExceptionHandlerFeature>();

                var exception = feature?.Error;

                if (exception is null)
                    return;

                logger.LogError(
                    exception,
                    "Unhandled exception occurred. Path: {Path}",
                    context.Request.Path
                );


                var problem = exception switch
                {
                    ValidationException validation =>
                        ProblemDetailsFactory.CreateValidationProblem(validation),

                    DomainException ex =>
                        ProblemDetailsFactory.CreateProblem(
                            StatusCodes.Status400BadRequest,
                            ex.Message),

                    BadRequestException ex =>
                        ProblemDetailsFactory.CreateProblem(
                            StatusCodes.Status400BadRequest,
                            ex.Message),

                    NotFoundException ex =>
                        ProblemDetailsFactory.CreateProblem(
                            StatusCodes.Status404NotFound,
                            ex.Message),

                    UnAuthorizedException ex =>
                        ProblemDetailsFactory.CreateProblem(
                            StatusCodes.Status401Unauthorized,
                            ex.Message),

                    _ =>
                        ProblemDetailsFactory.CreateProblem(
                            StatusCodes.Status500InternalServerError,
                            "An unexpected error occurred.")
                };

                context.Response.StatusCode = problem.Status ?? 500;

                await context.Response.WriteAsJsonAsync(
                    problem,
                    problem.GetType(), // ensures ValidationProblemDetails serializes Errors
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
            });
        });
    }
}