namespace ProductionControl.GraphQL.Service;

/// <summary>
/// The Product entity. This subgraph OWNS the Product type.
/// Its primary key in this subgraph is <see cref="Id"/> (see ProductQueries.GetProductById).
///
/// <see cref="PartNumber"/> is the field that links a Product to a Part in the
/// MasterData subgraph. Because MasterData also uses partNumber as a key for Product,
/// this field is marked [Shareable] (it is a non-key field here, but a key over there).
/// </summary>
public sealed class Product
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required string Category { get; init; }

    [Shareable]
    public required string PartNumber { get; init; }

    public required decimal Price { get; init; }
}
