using MasterData.Domain.Entities;
using MasterData.Domain.Interfaces;

namespace MasterData.Infrastructure.Repositories;

/// <summary>
/// In-memory PartGroup data. Replace with Dapper + Oracle or EF Core + Oracle in production.
/// </summary>
public sealed class PartGroupMockRepository : IPartGroupRepository
{
    private static readonly IReadOnlyList<PartGroup> PartGroups =
    [
        new PartGroup { Code = "PG-FAST", Name = "Fasteners",  Description = "Bolts, screws, nuts and washers" },
        new PartGroup { Code = "PG-BRKT", Name = "Brackets",   Description = "Structural mounting brackets" },
        new PartGroup { Code = "PG-SEAL", Name = "Seals",      Description = "Gaskets, O-rings and sealing elements" },
    ];

    public Task<PartGroup?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        => Task.FromResult(PartGroups.FirstOrDefault(g => g.Code == code));

    public Task<IEnumerable<PartGroup>> GetAllAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<IEnumerable<PartGroup>>(PartGroups);
}
