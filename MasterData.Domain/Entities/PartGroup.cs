namespace MasterData.Domain.Entities;

public sealed class PartGroup
{
    public required string Code { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
}
