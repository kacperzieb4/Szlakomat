using System;

namespace Szlakomat.Scoring.Domain.ValueObjects;

public record TrailCategory
{
    public string Value { get; }

    public TrailCategory(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Category cannot be null or whitespace.", nameof(value));
        }

        Value = value.Trim().ToLowerInvariant();
    }

    public override string ToString() => Value;
}
