namespace BuildingBlocks.Exceptions;

public abstract class ApplicationException : Exception
{
    protected ApplicationException(
        int statusCode,
        string detail,
        string? title = null,
        string? code = null
    )
        : base(title)
    {
        Title = title;
        StatusCode = statusCode;
        Detail = detail;
        Code = code;
    }

    public string? Title { get; }
    public int StatusCode { get; }
    public string Detail { get; }
    public string? Code { get; }
}