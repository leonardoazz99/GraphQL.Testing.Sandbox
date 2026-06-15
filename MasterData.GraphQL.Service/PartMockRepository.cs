namespace MasterData.GraphQL.Service;

public class PartRepository : IPartRepository
{
    private static readonly List<Part> Parts =
    [
        new Part { PartNumber = "PN-1001", Name = "Hex Bolt M8",      Material = "Stainless Steel A2", WeightGrams = 12.5, Standard = "DIN 933",  Manufacturer = "BoltCo" },
        new Part { PartNumber = "PN-1002", Name = "Steel Bracket 90", Material = "Carbon Steel",       WeightGrams = 85.0, Standard = "-",        Manufacturer = "BracketWorks" },
        new Part { PartNumber = "PN-1003", Name = "Rubber Gasket",    Material = "EPDM",               WeightGrams = 8.0,  Standard = "ISO 3601", Manufacturer = "SealTech" },
    ];

    Task<Part?> IPartRepository.GetByPartNumberAsync(string partNumber, CancellationToken cancellationToken)
        => Task.FromResult(Parts.FirstOrDefault(p => p.PartNumber == partNumber));

    Task<IEnumerable<Part>> IPartRepository.GetAllAsync(CancellationToken cancellationToken) => Task.FromResult((IEnumerable<Part>)Parts);
}
