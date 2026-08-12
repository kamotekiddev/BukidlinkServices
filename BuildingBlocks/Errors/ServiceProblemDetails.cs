using Microsoft.AspNetCore.Mvc;

namespace BuildingBlocks.Errors;

public sealed class ServiceProblemDetails : ProblemDetails
{
    public string? Code { get; init; }
}