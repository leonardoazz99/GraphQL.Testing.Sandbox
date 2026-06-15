namespace MasterData.Contracts;

public sealed record PartDto(
    string PartNumber,
    string Name,
    string Material,
    double WeightGrams,
    string Standard,
    string Manufacturer);