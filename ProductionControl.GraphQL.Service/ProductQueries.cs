namespace ProductionControl.GraphQL.Service;

[QueryType]
public static partial class ProductQueries
{
    /// <summary>All products. Plain list resolver.</summary>
    public static Task<IEnumerable<Product>> GetProductsAsync(
        [Service] IProductRepository repository,
        CancellationToken cancellationToken)
        => repository.GetAllAsync(cancellationToken);

    /// <summary>
    /// PUBLIC lookup for a single product by its key (id).
    /// Because it is public it shows up in the composite schema as `productById`,
    /// AND it acts as the gateway's entry point to resolve a Product by id.
    /// Lookups must return a NULLABLE entity so unresolved keys return null
    /// instead of failing the whole query.
    /// </summary>
    [Lookup]
    public static Task<Product?> GetProductByIdAsync(
        int id,
        [Service] IProductRepository repository,
        CancellationToken cancellationToken)
        => repository.GetByIdAsync(id, cancellationToken);
}
