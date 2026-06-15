namespace ProductionControl.GraphQL.Service;

[QueryType]
public static partial class PartQueries
{
    /// <summary>
    /// INTERNAL lookup for the Part entity, keyed by partNumber.
    ///
    /// The gateway uses this as the entry point into THIS subgraph when it needs to
    /// resolve the `products` field on a Part (whose master data lives in MasterData).
    /// It is [Internal], so it is hidden from the composite schema — clients cannot
    /// call `partByPartNumber` on this subgraph directly; only the gateway uses it.
    /// </summary>
    [Lookup, Internal]
    public static Part? GetPartByPartNumber(string partNumber)
        => new Part(partNumber);
}
