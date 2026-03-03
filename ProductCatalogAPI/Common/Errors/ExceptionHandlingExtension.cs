using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using ProductCatalogAPI.Common.Exceptions;

namespace ProductCatalogAPI.Common.Errors;

public static class ExceptionHandlingExtensions
{
    public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var feature = context.Features
                    .Get<IExceptionHandlerFeature>();

                var exception = feature?.Error;

                if (exception is null)
                    return;

                var problem = exception switch
                {
                    ValidationException validation =>
                        ProblemDetailsFactory.CreateValidationProblem(validation),

                    DomainException domain =>
                        ProblemDetailsFactory.CreateProblem(
                            StatusCodes.Status400BadRequest,
                            domain.Message),

                    KeyNotFoundException =>
                        ProblemDetailsFactory.CreateProblem(
                            StatusCodes.Status404NotFound,
                            "Resource not found"),

                    _ =>
                        ProblemDetailsFactory.CreateProblem(
                            StatusCodes.Status500InternalServerError,
                            "An unexpected error occurred.")
                };

                // context.Response.StatusCode = problem.Status ?? 500;
                // await context.Response.WriteAsJsonAsync(problem);
                context.Response.StatusCode = problem.Status ?? 500;
                await context.Response.WriteAsJsonAsync(
                    problem,
                    problem.GetType(), // ensures ValidationProblemDetails serializes Errors
                    new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
            });
        });
    }
}