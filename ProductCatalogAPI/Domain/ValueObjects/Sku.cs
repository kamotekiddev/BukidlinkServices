using System.Text.RegularExpressions;
using ProductCatalogAPI.Common.Errors;

namespace ProductCatalogAPI.Domain.ValueObjects;

public record Sku
{
    private const int MaxLength = 15;
    private const int MinLength = 5;
    private static readonly Regex AllowedPattern = new("^[A-Z0-9-]+$", RegexOptions.Compiled);

    public string Value { get; init; }

    private Sku(string value) => Value = value;

    public static Sku Create(string value)
    {
        if (value.Length > MaxLength) throw new InvalidSkuException($"SKU exceeds the max length {value}");
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidSkuException("SKU cannot be empty.");

        value = value.Trim().ToUpperInvariant();

        if (value.Length < MinLength) throw new InvalidSkuException($"SKU must be at least {MinLength} characters.");

        if (value.Length > MaxLength)
            throw new InvalidSkuException($"SKU cannot exceed {MaxLength} characters.");

        if (!AllowedPattern.IsMatch(value))
            throw new InvalidSkuException("SKU contains invalid characters.");

        return new Sku(value);
    }
};