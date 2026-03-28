namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents the data required to create or append an item to a sale aggregate.
/// </summary>
public sealed record SaleItemInput(
    string ProductExternalId,
    string ProductName,
    int Quantity,
    decimal UnitPrice);
