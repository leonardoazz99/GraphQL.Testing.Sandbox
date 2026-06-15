using MasterData.Domain.Entities;
using MasterData.Domain.Interfaces;

namespace MasterData.Infrastructure.Repositories;

/// <summary>
/// In-memory Part data. Replace with Dapper + Oracle or EF Core + Oracle in production.
/// </summary>
public sealed class PartMockRepository : IPartRepository
{
    private static readonly IReadOnlyList<Part> Parts =
    [
        new Part { PartNumber = "PN-1001", Name = "Hex Bolt M8",      Material = "Stainless Steel A2", WeightGrams = 12.5, Standard = "DIN 933",  Manufacturer = "BoltCo" },
        new Part { PartNumber = "PN-1002", Name = "Steel Bracket 90", Material = "Carbon Steel",       WeightGrams = 85.0, Standard = "-",        Manufacturer = "BracketWorks" },
        new Part { PartNumber = "PN-1003", Name = "Rubber Gasket",    Material = "EPDM",               WeightGrams = 8.0,  Standard = "ISO 3601", Manufacturer = "SealTech" },
    ];

    public Task<Part?> GetAsync(string partNumber, CancellationToken cancellationToken = default)
        => Task.FromResult(Parts.FirstOrDefault(p => p.PartNumber == partNumber));

    public Task<IEnumerable<Part>> GetAllAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<IEnumerable<Part>>(Parts);
}
