using Bogus;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

public static class CreateSaleHandlerTestData
{
    private static readonly Faker<CreateSaleItemCommand> ItemFaker = new Faker<CreateSaleItemCommand>()
        .RuleFor(item => item.ProductExternalId, faker => $"product-{faker.Random.Guid()}")
        .RuleFor(item => item.ProductName, faker => faker.Commerce.ProductName())
        .RuleFor(item => item.Quantity, faker => faker.Random.Int(1, 10))
        .RuleFor(item => item.UnitPrice, faker => decimal.Parse(faker.Commerce.Price(10, 200)));

    private static readonly Faker<CreateSaleCommand> CommandFaker = new Faker<CreateSaleCommand>()
        .RuleFor(command => command.SaleDate, faker => faker.Date.RecentOffset(30).UtcDateTime)
        .RuleFor(command => command.CustomerExternalId, faker => $"customer-{faker.Random.Guid()}")
        .RuleFor(command => command.CustomerName, faker => faker.Person.FullName)
        .RuleFor(command => command.BranchExternalId, faker => $"branch-{faker.Random.Guid()}")
        .RuleFor(command => command.BranchName, faker => faker.Company.CompanyName())
        .RuleFor(command => command.Items, _ => [ItemFaker.Generate(), ItemFaker.Generate()]);

    public static CreateSaleCommand GenerateValidCommand()
    {
        return CommandFaker.Generate();
    }
}
