using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Products.Common;

public static class ListProductsOrderParser
{
    private static readonly HashSet<string> SupportedFields =
    [
        "id",
        "title",
        "price",
        "description",
        "category",
        "image",
        "rate",
        "count",
        "rating.rate",
        "rating.count"
    ];

    public static string Normalize(string? order)
    {
        if (string.IsNullOrWhiteSpace(order))
        {
            return ListProductsDefaults.DefaultOrder;
        }

        return order.Trim().Trim('"');
    }

    public static bool IsSupported(string? order)
    {
        try
        {
            Parse(order);
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    public static IReadOnlyList<ProductSortField> Parse(string? order)
    {
        var normalized = Normalize(order);
        var fields = normalized
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(ParseField)
            .ToList();

        if (fields.Count == 0)
        {
            return [new ProductSortField("id", false)];
        }

        return fields;
    }

    private static ProductSortField ParseField(string rawField)
    {
        var parts = rawField.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length is 0 or > 2)
        {
            throw new ArgumentException($"Unsupported sort order '{rawField}'.", nameof(rawField));
        }

        var field = parts[0];
        if (!SupportedFields.Contains(field))
        {
            throw new ArgumentException($"Unsupported sort field '{field}'.", nameof(rawField));
        }

        var descending = parts.Length == 2 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);
        var hasSupportedDirection = parts.Length == 1 || descending || parts[1].Equals("asc", StringComparison.OrdinalIgnoreCase);
        if (!hasSupportedDirection)
        {
            throw new ArgumentException($"Unsupported sort direction in '{rawField}'.", nameof(rawField));
        }

        var normalizedField = field switch
        {
            "rating.rate" => "rate",
            "rating.count" => "count",
            _ => field
        };

        return new ProductSortField(normalizedField, descending);
    }
}
