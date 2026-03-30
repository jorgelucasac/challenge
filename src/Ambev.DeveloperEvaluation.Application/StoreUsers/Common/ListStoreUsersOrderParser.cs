using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.StoreUsers.Common;

public static class ListStoreUsersOrderParser
{
    private static readonly HashSet<string> SupportedFields = ["id", "email", "username", "password", "phone", "status", "role"];

    public static string Normalize(string? order)
    {
        if (string.IsNullOrWhiteSpace(order))
        {
            return ListStoreUsersDefaults.DefaultOrder;
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

    public static IReadOnlyList<StoreUserSortField> Parse(string? order)
    {
        var normalized = Normalize(order);
        var fields = normalized
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(ParseField)
            .ToList();

        return fields.Count == 0 ? [new StoreUserSortField("id", false)] : fields;
    }

    private static StoreUserSortField ParseField(string rawField)
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

        return new StoreUserSortField(field, descending);
    }
}
