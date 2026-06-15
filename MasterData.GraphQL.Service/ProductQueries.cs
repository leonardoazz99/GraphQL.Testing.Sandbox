namespace MasterData.GraphQL.Service;

[QueryType]
public static partial class ProductQueries
{
    /// <summary>
    /// INTERNAL lookup for the Product entity, keyed by partNumber.
    ///
    /// The gateway uses this to enter THIS subgraph when it needs the `part` field on a
    /// Product. It is [Internal] so clients cannot call it directly — the partNumber is
    /// already known from the Products subgraph, so we just build a stub with `new(partNumber)`.
    /// </summary>
    [Lookup, Internal]
    public static Product? GetProductByPartNumber(string partNumber)
        => new Product(partNumber);
}
