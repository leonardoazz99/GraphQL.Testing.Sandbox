namespace MasterData.GraphQL.Service;

/// <summary>
/// The Part (master data) entity. This subgraph OWNS the Part type.
/// Its key is <see cref="PartNumber"/> (see PartQueries.GetPartByPartNumber).
/// </summary>
public sealed class Part
{
    public required string PartNumber { get; init; }

    public required string Name { get; init; }

    public required string Material { get; init; }

    /// <summary>Weight in grams.</summary>
    public required double WeightGrams { get; init; }

    public required string Standard { get; init; }

    public required string Manufacturer { get; init; }
}
