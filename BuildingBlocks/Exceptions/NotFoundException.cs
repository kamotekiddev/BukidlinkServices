namespace BuildingBlocks.Exceptions;

public class NotFoundException(string message) : KeyNotFoundException(message);