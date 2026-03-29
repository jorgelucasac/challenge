namespace Ambev.DeveloperEvaluation.Functional.Infrastructure;

[CollectionDefinition(CollectionName)]
public sealed class FunctionalApiCollection : ICollectionFixture<FunctionalApiFactory>
{
    public const string CollectionName = "functional-api";
}
