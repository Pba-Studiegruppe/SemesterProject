using System;

namespace Assignment_Domain.Entities;

public partial class ErrorType
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public byte[]? RowVersion { get; set; }

    // EF
    private ErrorType() { }

    public ErrorType(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        Id = Guid.NewGuid();
        Name = name.Trim();
        Description = description;
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        Name = name.Trim();
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }
}