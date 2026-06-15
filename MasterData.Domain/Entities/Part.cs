namespace MasterData.Domain.Entities;

public sealed class Part
{
    public required string PartNumber { get; init; }
    public required string Name { get; init; }
    public required string Material { get; init; }
    public required double WeightGrams { get; init; }
    public required string Standard { get; init; }
    public required string Manufacturer { get; init; }
}
