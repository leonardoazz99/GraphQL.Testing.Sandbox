namespace MasterData.GraphQL.Service;

/// <summary>
/// Entity STUB for the Product type, which is OWNED by the Products subgraph.
///
/// Here we key Product by its <c>partNumber</c> and contribute ONE field: <c>part</c>,
/// the master-data Part associated with the product. The gateway merges this stub with
/// the full Product type from the Products subgraph, so clients get `product.part`.
///
/// `partNumber` is the key here; in the Products subgraph it is a regular field marked
/// [Shareable]. Key fields are automatically shareable, so no attribute is needed on this side.
/// </summary>
public sealed record Product(string PartNumber)
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Part?> GetPartAsync(
        [Service] IPartRepository repository,
        CancellationToken cancellationToken)
        => repository.GetByPartNumberAsync(PartNumber, cancellationToken);
}