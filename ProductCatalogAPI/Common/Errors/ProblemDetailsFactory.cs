using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ProductCatalogAPI.Common.Errors;

public abstract class ProblemDetailsFactory
{
    public static ProblemDetails CreateProblem(int status, string title)
    {
        return new ProblemDetails
        {
            Status = status,
            Title = title,
            Type = $"https://httpstatuses.com/{status}"
        };
    }

    public static ValidationProblemDetails CreateValidationProblem(
        ValidationException validationException)
    {
        var errors = validationException.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray());

        return new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed",
        };
    }


}