using BuildingBlocks.Entities;
using BuildingBlocks.Exceptions;
using StoreAPI.Domain.ValueObjects;

namespace StoreAPI.Domain;

public enum StoreStatus
{
    Active,
    Inactive,
    Suspended
}

public class Store : Entity
{
    private Store()
    {
    }

    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Address Address { get; private set; }
    public Guid OwnerId { get; init; }
    public StoreStatus Status { get; private set; }

    public static Store Create(string name, string? description, Guid ownerId, Address address)
    {
        if (ownerId == Guid.Empty) throw new DomainException("Invalid owner id");

        return new Store
        {
            Name = NormalizeString(name),
            Description = description?.Trim(),
            OwnerId = ownerId,
            Address = address,
            Status = StoreStatus.Inactive
        };
    }

    public void Rename(string name)
    {
        Name = NormalizeString(name);
    }

    public void UpdateDescription(string description)
    {
        Description = NormalizeString(description);
    }

    public void ChangeAddress(Address address)
    {
        Address = address;
    }


    public void Activate()
    {
        Status = StoreStatus.Active;
    }

    public void Deactivate()
    {
        Status = StoreStatus.Inactive;
    }

    public void Suspend()
    {
        Status = StoreStatus.Suspended;
    }

    private static string NormalizeString(string str)
    {
        if (string.IsNullOrWhiteSpace(str)) throw new DomainException("Invalid input.");
        return str.Trim();
    }
}