using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Carts.Common;

public static class ListCartsOrderParser
{
    private static readonly HashSet<string> SupportedFields = ["id", "userId", "date"];

    public static string Normalize(string? order)
    {
        if (string.IsNullOrWhiteSpace(order))
        {
            return ListCartsDefaults.DefaultOrder;
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

    public static IReadOnlyList<CartSortField> Parse(string? order)
    {
        var normalized = Normalize(order);
        var fields = normalized
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(ParseField)
            .ToList();

        return fields.Count == 0 ? [new CartSortField("id", false)] : fields;
    }

    private static CartSortField ParseField(string rawField)
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

        return new CartSortField(field, descending);
    }
}
