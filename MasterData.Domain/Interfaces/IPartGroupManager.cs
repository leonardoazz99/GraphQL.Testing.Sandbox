using MasterData.Domain.Entities;

namespace MasterData.Domain.Interfaces;

public interface IPartGroupManager
{
    Task<PartGroup?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<PartGroup>> GetAllAsync(CancellationToken cancellationToken = default);
}
