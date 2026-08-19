using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Exceptions;

public class DomainException : ApplicationException
{
    public DomainException(string detail) : base(StatusCodes.Status409Conflict, detail)
    {
    }

    public DomainException(int statusCode, string detail, string title, string? code = null) :
        base(statusCode, detail, title, code)
    {
    }

    public DomainException(int statusCode, string detail, string? code = null) :
        base(statusCode, detail, null, code)
    {
    }
}