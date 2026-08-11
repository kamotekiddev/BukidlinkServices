namespace BuildingBlocks.Exceptions;

public class ConflictException(string message, string? code = null) : Exception(message)
{
    public string? Code { get; set; } = code;
}