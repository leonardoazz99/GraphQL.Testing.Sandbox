namespace ProductionControl.GraphQL.Service;

/// <summary>
/// Entity STUB for the Part type, which is OWNED by the MasterData subgraph.
///
/// This subgraph does not know anything about a Part's master data (material, weight, ...).
/// It only contributes one extra field to the Part type: <c>products</c> — the list of
/// products that reference this part number.
///
/// At composition time the gateway merges this stub with the full Part type from MasterData,
/// so clients see a single Part type with both its master data and its products.
/// </summary>
public sealed record Part(string PartNumber)
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<IEnumerable<Product>> GetProductsAsync(
        [Service] IProductRepository repository,
        CancellationToken cancellationToken)
        => repository.GetByPartNumberAsync(PartNumber, cancellationToken);
}
