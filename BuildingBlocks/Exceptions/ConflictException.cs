using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Exceptions;

public class ConflictException : ApplicationException
{
    public ConflictException(int statusCode, string detail, string? title = null, string? code = null) :
        base(statusCode, detail, title, code)
    {
    }

    public ConflictException(string detail, string? code = null) :
        base(StatusCodes.Status409Conflict, detail, null, code)
    {
    }
}