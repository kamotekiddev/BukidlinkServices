using BuildingBlocks.Exceptions;

namespace StoreAPI.Domain.ValueObjects;

public sealed class Address
{
    private Address()
    {
    }

    public string? AddressLine1 { get; private set; }
    public string? AddressLine2 { get; private set; }
    public string City { get; private set; }
    public string Province { get; private set; }
    public string ZipCode { get; private set; }

    public static Address Create(string? addressLine1, string? addressLine2, string city, string province,
        string zipcode)
    {
        return new Address
        {
            AddressLine1 = addressLine1?.Trim(),
            AddressLine2 = addressLine2?.Trim(),
            City = NormalizeString(city),
            Province = NormalizeString(province),
            ZipCode = NormalizeString(zipcode)
        };
    }

    private static string NormalizeString(string str)
    {
        if (string.IsNullOrWhiteSpace(str)) throw new DomainException("Invalid input.");
        return str.Trim();
    }
}